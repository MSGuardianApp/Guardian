using DotSpatial.Data;
using DotSpatial.Projections;
using Guardian.Common.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SOS.Service.Utility
{
    public class ShapeFileGISutility
    {
        Settings settings;
        public ShapeFileGISutility(Settings settings)
        {
            this.settings = settings;
        }
        public CloudBlobContainer container { get; set; }

        public List<string> GetAllWardNames(string PathPrefix, string SubGroupIdentificationKey)
        {

            string connectionString = settings.AzureStorageConnectionString;
            var _StorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);
            var blobClient = _StorageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("shapes");

            var ShapeBlockBlobReference = container.GetBlockBlobReference(this.GetPath(FileType.ShapeFile, PathPrefix, container.Uri.GetLeftPart(UriPartial.Path)));
            ShapeBlockBlobReference.DownloadToFile(PathPrefix + ".shp", FileMode.Create);

            var featureSet = Shapefile.Open(PathPrefix + ".shp");

            DataTable dt = featureSet.DataTable;

            List<string> subGroupNames = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                subGroupNames.Add(row[SubGroupIdentificationKey].ToString());
            }
            
            return subGroupNames.Where(sgn => !string.IsNullOrEmpty(sgn)).ToList();
        }
        private string GetPath(FileType type, string PathPrefix, string PluginBasePath)
        {
            //var attribute = (ShapeFileAttribute)this.GISFileAttribute;

            switch (type)
            {
                case FileType.ShapeFile:
                    return PathPrefix + ".shp";

                case FileType.ProjectionFile:
                    return PathPrefix + ".prj";

                case FileType.DescribeFile:
                    return PathPrefix + ".dbf";

                case FileType.IndexFile:
                    return PathPrefix + ".shx";
            }

            throw new InvalidOperationException("Unsupported file type!");
        }
        public static DataRow FindIntersection(double latitude, double longitude, string ShapePath)
        {
            DataRow returnValue = null;

            if (!string.IsNullOrWhiteSpace(ShapePath))
            {

                string ShapeFilePath = Path.GetFileNameWithoutExtension(ShapePath);
                var featureSet = Shapefile.Open(ShapePath + ".shp");
                var coordinate = Project(ShapePath + ".prj", longitude, latitude);
                if (double.IsNaN(coordinate[0]) || double.IsNaN(coordinate[1]))
                {
                    return returnValue;
                }

                foreach (var feature in featureSet.Features)
                {
                    if (feature.ShapeIndex.Intersects(new DotSpatial.Topology.Coordinate(coordinate)))
                    {
                        return feature.DataRow;
                    }
                }
            }

            return returnValue;
        }

        private static double[] Project(string projectionFilePath, double longitude, double latitude)
        {
            var srcProjection = KnownCoordinateSystems.Geographic.World.WGS1984;
            var destProjection = new ProjectionInfo();
            var xy = new double[] { longitude, latitude };
            var z = new double[] { 1 };

            destProjection.ParseEsriString(File.ReadAllText(projectionFilePath));
            Reproject.ReprojectPoints(xy, z, srcProjection, destProjection, 0, 1);

            return xy;
        }


        private enum FileType
        {
            ShapeFile,
            ProjectionFile,
            DescribeFile,
            IndexFile
        }
    }
}
