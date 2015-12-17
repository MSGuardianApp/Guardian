//
//  PhoneBuddy.m
//  Guardian
//
//  Created by PTG on 19/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "PhoneBuddy.h"

@implementation PhoneBuddy

- (void)encodeWithCoder:(NSCoder *)encoder
{
    [encoder encodeObject:self.BuddyId forKey:@"BuddyId"];
    [encoder encodeObject:self.firstName forKey:@"firstName"];
    [encoder encodeObject:self.lastName forKey:@"lastName"];
    [encoder encodeObject:self.ProfilePic forKey:@"ProfilePic"];
    [encoder encodeObject:self.mobileNumber forKey:@"mobileNumber"];
    [encoder encodeObject:self.homeNumber forKey:@"homeNumber"];
    [encoder encodeObject:self.homeNumber forKey:@"Email"];
    [encoder encodeObject:self.ToRemove forKey:@"ToRemove"];
    [encoder encodeObject:self.UserID forKey:@"UserID"];
    [encoder encodeObject:self.UserID forKey:@"IsPrimeBuddy"];
    [encoder encodeObject:self.state forKey:@"state"];
    
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    if(self = [super init])
    {
        self.BuddyId = [aDecoder decodeObjectForKey:@"BuddyId"];
        self.firstName = [aDecoder decodeObjectForKey:@"firstName"];
        self.lastName = [aDecoder decodeObjectForKey:@"lastName"];
        self.ProfilePic = [aDecoder decodeObjectForKey:@"ProfilePic"];
        self.mobileNumber = [aDecoder decodeObjectForKey:@"mobileNumber"];
        self.homeNumber = [aDecoder decodeObjectForKey:@"homeNumber"];
        self.Email = [aDecoder decodeObjectForKey:@"Email"];
        self.ToRemove = [aDecoder decodeObjectForKey:@"ToRemove"];
        self.UserID = [aDecoder decodeObjectForKey:@"UserID"];
        self.IsPrimeBuddy = [aDecoder decodeObjectForKey:@"IsPrimeBuddy"];
        self.state = [aDecoder decodeObjectForKey:@"state"];
    }
    return self;
}
@end
