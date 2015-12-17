//
//  PhoneSetting.m
//  Guardian
//
//  Created by PTG on 24/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "PhoneSetting.h"

@implementation PhoneSetting


- (void)encodeWithCoder:(NSCoder *)encoder
{
    [encoder encodeObject:self.CanEmail forKey:@"CanEmail"];
    [encoder encodeObject:self.CanSMS forKey:@"CanSMS"];
    [encoder encodeObject:self.DeviceID forKey:@"DeviceID"];
    [encoder encodeObject:self.ProfileID forKey:@"ProfileID"];
    [encoder encodeObject:self.PlatForm forKey:@"PlatForm"];
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    if(self = [super init])
    {
        self.CanEmail = [aDecoder decodeObjectForKey:@"CanEmail"];
        self.CanSMS = [aDecoder decodeObjectForKey:@"CanSMS"];
        self.DeviceID = [aDecoder decodeObjectForKey:@"DeviceID"];
        self.ProfileID = [aDecoder decodeObjectForKey:@"ProfileID"];
        self.PlatForm = [aDecoder decodeObjectForKey:@"PlatForm"];
    }
    return self;
}


@end
