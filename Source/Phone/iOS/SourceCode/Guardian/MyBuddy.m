//
//  MyBuddy.m
//  Guardian
//
//  Created by PTG on 24/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "MyBuddy.h"

@implementation MyBuddy



- (void)encodeWithCoder:(NSCoder *)encoder
{
    [encoder encodeObject:self.DataInfo forKey:@"DataInfo"];
    [encoder encodeObject:self.Email forKey:@"Email"];
    [encoder encodeObject:self.FBAuthID forKey:@"FBAuthID"];
    [encoder encodeObject:self.FBID forKey:@"FBID"];
    [encoder encodeObject:self.objLiveDetails forKey:@"objLiveDetails"];
    [encoder encodeObject:self.MobileNumber forKey:@"MobileNumber"];
    [encoder encodeObject:self.Name forKey:@"Name"];
    [encoder encodeObject:self.RegionCode forKey:@"RegionCode"];
    [encoder encodeObject:self.UserID forKey:@"UserID"];
    [encoder encodeObject:self.BuddyID forKey:@"BuddyID"];
    [encoder encodeObject:self.State forKey:@"State"];
    [encoder encodeObject:self.IsPrimeBuddy forKey:@"IsPrimeBuddy"];
    [encoder encodeObject:self.ToRemove forKey:@"ToRemove"];
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    if(self = [super init])
    {
        self.DataInfo = [aDecoder decodeObjectForKey:@"DataInfo"];
        self.Email = [aDecoder decodeObjectForKey:@"Email"];
        self.FBAuthID = [aDecoder decodeObjectForKey:@"FBAuthID"];
        self.FBID = [aDecoder decodeObjectForKey:@"FBID"];
        self.objLiveDetails = [aDecoder decodeObjectForKey:@"objLiveDetails"];
        self.MobileNumber = [aDecoder decodeObjectForKey:@"MobileNumber"];
        self.Name = [aDecoder decodeObjectForKey:@"Name"];
        self.RegionCode = [aDecoder decodeObjectForKey:@"RegionCode"];
        self.UserID = [aDecoder decodeObjectForKey:@"UserID"];
        self.BuddyID = [aDecoder decodeObjectForKey:@"BuddyID"];
        self.State = [aDecoder decodeObjectForKey:@"State"];
        self.IsPrimeBuddy = [aDecoder decodeObjectForKey:@"IsPrimeBuddy"];
        self.ToRemove = [aDecoder decodeObjectForKey:@"ToRemove"];
    }
    return self;
}


@end
