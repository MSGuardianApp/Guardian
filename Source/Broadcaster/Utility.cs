using System;
using System.Collections.Generic;
using System.IO;
using SOS.ConfigManager;
using SOS.Service.Implementation;
using SOS.AzureStorageAccessLayer;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace SOS.WorkerRole.Broadcaster
{
    public static class Utility
    {       
        private static string _smsPanicMessage = ConfigManager.Resources.Messages.SMSMessage;
        private static string _emailPanicMessage = ConfigManager.Resources.Messages.EmailMessage;
        private static string _facebookPanicMessage = ConfigManager.Resources.Messages.FacebookMessage;
        private static string _emailPanicSubject = ConfigManager.Resources.Messages.EmailSubject;
        private static string _facebookPanicSubject = ConfigManager.Resources.Messages.FacebookSubject;

        public static string GetSMSBody(string TinyURL, string UserName, string UserAddress, string UserPhoneNumber)
        {
            return _smsPanicMessage.Replace("{name}", UserName).Replace("{phone}", UserPhoneNumber).Replace("{address}", UserAddress).Replace("{tinyuri}", TinyURL);
        }

        public static string GetEmailBody(string TinyURL, string UserName, string UserAddress, string UserPhoneNumber, DateTime time)
        {
            return _emailPanicMessage.Replace("{name}", UserName).Replace("{phone}", UserPhoneNumber).Replace("{address}", UserAddress).Replace("{tinyuri}", TinyURL).Replace("{time}", time.ToLongDateString() + " " + time.ToLongTimeString());
        }

        public static string GetEmailSubject(string UserName)
        {
            return _emailPanicSubject.Replace("{name}", UserName);
        }

        public static string GetFacebookMessage(string TinyURL, string UserName, string UserAddress, string UserPhoneNumber, DateTime time)
        {
            return _facebookPanicMessage.Replace("{name}", UserName).Replace("{phone}", UserPhoneNumber).Replace("{address}", UserAddress).Replace("{tinyuri}", TinyURL).Replace("{time}", time.ToLongDateString() + " " + time.ToLongTimeString());
        }

        public static string GetFacebookCaption(string UserName)
        {
            return _facebookPanicSubject.Replace("{name}", UserName);
        }
        private enum FileType
        {
            ShapeFile,
            ProjectionFile,
            DescribeFile,
            IndexFile
        }

        private static string LocalPath(LocalResource myStorage, String FileName)
        {
            return Path.Combine(myStorage.RootPath, FileName);
        }
        private static string GetPath(FileType type, string PathPrefix)
        {
            if (type == FileType.ShapeFile)
            {
                return PathPrefix + ".shp";
            }
            else if (type == FileType.ProjectionFile)
            {
                return PathPrefix + ".prj";
            }
            else if (type == FileType.DescribeFile)
            {
                return PathPrefix + ".dbf";

            }
            else if (type == FileType.IndexFile)
            {
                return PathPrefix + ".shx";
            }

            throw new InvalidOperationException("Unsupported file type!");
        }

        public static DateTime LastUpdateDate;
        private static List<Tuple<int, string, string>> _GrpIdGroupKeyAndShapePaths;
        public static List<Tuple<int, string, string>> GrpIdGroupKeyAndShapePaths
        {

            get
            {
                bool isCacheResetRequired = false;

                if (LastUpdateDate == DateTime.MinValue)
                    isCacheResetRequired = true;
                else
                {
                    if (DateTime.UtcNow.Subtract(LastUpdateDate).TotalMinutes >= Config.TimeToResetCacheInMinutes)
                    {
                        isCacheResetRequired = true;
                    }
                }

                if (isCacheResetRequired)
                {
                    _GrpIdGroupKeyAndShapePaths = GetGroupIdWithKeysAndShapeIds();
                    LastUpdateDate = DateTime.UtcNow;
                }
                return _GrpIdGroupKeyAndShapePaths;
            }
        }


        static List<Tuple<int, string, string>> GetGroupIdWithKeysAndShapeIds()
        {
            GroupService grpService = new GroupService();
            BlobAccess blobAccess = new BlobAccess();
            var container = blobAccess.LoadShapeFiles();
            LocalResource myStorage = RoleEnvironment.GetLocalResource("LocalStorageWorkerRole");
            List<Tuple<int, string, string>> GrpIdGroupKeyAndShapePaths = new List<Tuple<int, string, string>>();

            GroupService.ParentGroup.ForEach(ParentGroup =>
            {

                if (!String.IsNullOrWhiteSpace(ParentGroup.ShapeFileID) && ParentGroup.NotifySubgroups) //
                {

                    string shapeIndex = LocalPath(myStorage, ParentGroup.ShapeFileID + ".shx");
                    string describeFile = LocalPath(myStorage, ParentGroup.ShapeFileID + ".dbf");
                    string shapeFile = LocalPath(myStorage, ParentGroup.ShapeFileID + ".shp");
                    string projectionFile = LocalPath(myStorage, ParentGroup.ShapeFileID + ".prj");

                    if (File.Exists(shapeIndex))
                        File.Delete(shapeIndex);

                    if (File.Exists(describeFile))
                        File.Delete(describeFile);


                    if (File.Exists(shapeFile))
                        File.Delete(shapeFile);

                    if (File.Exists(projectionFile))
                        File.Delete(projectionFile);

                    var ShxBlockBlobReference = container.GetBlockBlobReference(GetPath(FileType.IndexFile, ParentGroup.ShapeFileID));
                    ShxBlockBlobReference.DownloadToFile(shapeIndex, FileMode.Create);


                    var DescribeBlockBlobReference = container.GetBlockBlobReference(GetPath(FileType.DescribeFile, ParentGroup.ShapeFileID));
                    DescribeBlockBlobReference.DownloadToFile(describeFile, FileMode.Create);


                    var ShapeBlockBlobReference = container.GetBlockBlobReference(GetPath(FileType.ShapeFile, ParentGroup.ShapeFileID));
                    ShapeBlockBlobReference.DownloadToFile(shapeFile, FileMode.Create);

                    var BlockBlobReference = container.GetBlockBlobReference(GetPath(FileType.ProjectionFile, ParentGroup.ShapeFileID));
                    BlockBlobReference.DownloadToFile(projectionFile, FileMode.Create);

                    GrpIdGroupKeyAndShapePaths.Add(new Tuple<int, string, string>(ParentGroup.GroupID, ParentGroup.SubGroupIdentificationKey, Path.Combine(myStorage.RootPath, ParentGroup.ShapeFileID)));
                }

            });

            return GrpIdGroupKeyAndShapePaths;
        }
    }
}


#region Scrap Code
//Parallel.ForEach(GroupService.ParentGroup, opt, ParentGroup =>
//                 {
//                     if (!String.IsNullOrWhiteSpace(ParentGroup.ShapeFileID) && ParentGroup.NotifySubgroups) //
//                     {
//                         if (!File.Exists(ParentGroup.ShapeFileID + ".shx"))
//                         {
//                             var ShxBlockBlobReference = container.GetBlockBlobReference(GetPath(FileType.IndexFile, ParentGroup.ShapeFileID));
//                             ShxBlockBlobReference.DownloadToFile(LocalPath(myStorage, ParentGroup.ShapeFileID + ".shx"), FileMode.Create);
//                         }

//                         if (!File.Exists(ParentGroup.ShapeFileID + ".dbf"))
//                         {
//                             var DescribeBlockBlobReference = container.GetBlockBlobReference(GetPath(FileType.DescribeFile, ParentGroup.ShapeFileID));
//                             DescribeBlockBlobReference.DownloadToFile(LocalPath(myStorage, ParentGroup.ShapeFileID + ".dbf"), FileMode.Create);
//                         }

//                         if (!File.Exists(ParentGroup.ShapeFileID + ".shp"))
//                         {
//                             var ShapeBlockBlobReference = container.GetBlockBlobReference(GetPath(FileType.ShapeFile, ParentGroup.ShapeFileID));
//                             ShapeBlockBlobReference.DownloadToFile(LocalPath(myStorage, ParentGroup.ShapeFileID + ".shp"), FileMode.Create);
//                         }

//                         if (!File.Exists(ParentGroup.ShapeFileID + ".prj"))
//                         {
//                             var BlockBlobReference = container.GetBlockBlobReference(GetPath(FileType.ProjectionFile, ParentGroup.ShapeFileID));
//                             BlockBlobReference.DownloadToFile(LocalPath(myStorage, ParentGroup.ShapeFileID + ".prj"), FileMode.Create);
//                         }

//                         GrpIdGroupKeyAndShapePaths.Add(new Tuple<int, string, string>(ParentGroup.GroupID, ParentGroup.GroupKey, Path.Combine(myStorage.RootPath, ParentGroup.ShapeFileID)));
//                     }

//                 });
#endregion