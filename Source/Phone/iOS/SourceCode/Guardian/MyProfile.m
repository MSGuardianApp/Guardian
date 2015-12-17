//
//  MyProfile.m
//  Guardian
//
//  Created by PTG on 24/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "MyProfile.h"

@implementation MyProfile



- (void)encodeWithCoder:(NSCoder *)encoder
{
    
    [encoder encodeObject:self.AscGroups forKey:@"AscGroups"];
    [encoder encodeObject:self.CanArchive forKey:@"CanArchive"];
    [encoder encodeObject:self.CanMail forKey:@"CanMail"];
    [encoder encodeObject:self.CanPost forKey:@"CanPost"];
    [encoder encodeObject:self.CanSMS forKey:@"CanSMS"];
    [encoder encodeObject:self.CanSMS forKey:@"DataInfo"];
    [encoder encodeObject:self.Email forKey:@"Email"];
    [encoder encodeObject:self.FBAuthID forKey:@"FBAuthID"];
    [encoder encodeObject:self.FBGroupID forKey:@"FBGroupID"];
    [encoder encodeObject:self.FBGroupName forKey:@"FBGroupName"];
    [encoder encodeObject:self.FBID forKey:@"FBID"];
    [encoder encodeObject:self.IsSOSOn forKey:@"IsSOSOn"];
    [encoder encodeObject:self.IsTrackingOn forKey:@"IsTrackingOn"];
    [encoder encodeObject:self.IsValid forKey:@"IsValid"];
    [encoder encodeObject:self.LastLocs forKey:@"LastLocs"];
    [encoder encodeObject:self.objLiveDetails forKey:@"objLiveDetails"];
    [encoder encodeObject:self.LocateBuddies forKey:@"LocateBuddies"];
    [encoder encodeObject:self.LocationConsent forKey:@"LocationConsent"];
    [encoder encodeObject:self.MobileNumber forKey:@"MobileNumber"];
    [encoder encodeObject:self.MyBuddies forKey:@"MyBuddies"];
    [encoder encodeObject:self.objPhoneSetting forKey:@"objPhoneSetting"];
    [encoder encodeObject:self.Name forKey:@"Name"];
    [encoder encodeObject:self.PrimeGroupID forKey:@"PrimeGroupID"];
    [encoder encodeObject:self.ProfileID forKey:@"ProfileID"];
    [encoder encodeObject:self.RegionCode forKey:@"RegionCode"];
    [encoder encodeObject:self.SMSText forKey:@"SMSText"];
    [encoder encodeObject:self.SOSToken forKey:@"SOSToken"];
    [encoder encodeObject:self.SecurityToken forKey:@"SecurityToken"];
    [encoder encodeObject:self.SiteSetting forKey:@"SiteSetting"];
    [encoder encodeObject:self.TinyURI forKey:@"TinyURI"];
    [encoder encodeObject:self.SessionID forKey:@"SessionID"];
    [encoder encodeObject:self.UserID forKey:@"UserID"];
    [encoder encodeObject:self.UserID forKey:@"policeNum"];
    [encoder encodeObject:self.UserID forKey:@"ambulanceNum"];
    [encoder encodeObject:self.UserID forKey:@"fireNum"];
    
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    if(self = [super init])
    {
        self.AscGroups = [aDecoder decodeObjectForKey:@"AscGroups"];
        self.CanArchive = [aDecoder decodeObjectForKey:@"CanArchive"];
        self.CanMail = [aDecoder decodeObjectForKey:@"CanMail"];
        self.CanPost = [aDecoder decodeObjectForKey:@"CanPost"];
        self.CanSMS = [aDecoder decodeObjectForKey:@"CanSMS"];
        self.DataInfo = [aDecoder decodeObjectForKey:@"DataInfo"];
        self.Email = [aDecoder decodeObjectForKey:@"Email"];
        self.FBAuthID = [aDecoder decodeObjectForKey:@"FBAuthID"];
        self.FBGroupID = [aDecoder decodeObjectForKey:@"FBGroupID"];
        self.FBGroupName = [aDecoder decodeObjectForKey:@"FBGroupName"];
        self.FBID = [aDecoder decodeObjectForKey:@"FBID"];
        self.IsSOSOn = [aDecoder decodeObjectForKey:@"IsSOSOn"];
        self.IsTrackingOn = [aDecoder decodeObjectForKey:@"IsTrackingOn"];
        self.IsValid = [aDecoder decodeObjectForKey:@"IsValid"];
        self.LastLocs = [aDecoder decodeObjectForKey:@"LastLocs"];
        self.objLiveDetails = [aDecoder decodeObjectForKey:@"objLiveDetails"];
        self.LocateBuddies = [aDecoder decodeObjectForKey:@"LocateBuddies"];
        self.LocationConsent = [aDecoder decodeObjectForKey:@"LocationConsent"];
        self.MobileNumber = [aDecoder decodeObjectForKey:@"MobileNumber"];
        self.MyBuddies = [aDecoder decodeObjectForKey:@"MyBuddies"];
        self.objPhoneSetting = [aDecoder decodeObjectForKey:@"objPhoneSetting"];
        self.Name = [aDecoder decodeObjectForKey:@"Name"];
        self.PrimeGroupID = [aDecoder decodeObjectForKey:@"PrimeGroupID"];
        self.ProfileID = [aDecoder decodeObjectForKey:@"ProfileID"];
        self.RegionCode = [aDecoder decodeObjectForKey:@"RegionCode"];
        self.SMSText = [aDecoder decodeObjectForKey:@"SMSText"];
        self.SOSToken = [aDecoder decodeObjectForKey:@"SOSToken"];
        self.SecurityToken = [aDecoder decodeObjectForKey:@"SecurityToken"];
        self.SiteSetting = [aDecoder decodeObjectForKey:@"SiteSetting"];
        self.TinyURI = [aDecoder decodeObjectForKey:@"TinyURI"];
        self.SessionID = [aDecoder decodeObjectForKey:@"SessionID"];
        self.UserID = [aDecoder decodeObjectForKey:@"UserID"];
        self.policeNum = [aDecoder decodeObjectForKey:@"policeNum"];
        self.ambulanceNum = [aDecoder decodeObjectForKey:@"ambulanceNum"];
        self.fireNum = [aDecoder decodeObjectForKey:@"fireNum"];
        

            }
    return self;
}

@end
