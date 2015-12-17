//
//  User.m
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "User.h"

@implementation User


- (void)encodeWithCoder:(NSCoder *)encoder
{
    [encoder encodeObject:self.UserId forKey:@"UserId"];
    [encoder encodeObject:self.Name forKey:@"Name"];
    [encoder encodeObject:self.LiveEmail forKey:@"LiveEmail"];
    [encoder encodeObject:self.LiveAuthId forKey:@"LiveAuthId"];
    [encoder encodeObject:self.FBAuthId forKey:@"FBAuthId"];
    [encoder encodeObject:self.CurrentProfileId forKey:@"CurrentProfileId"];
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    if(self = [super init])
    {
        self.UserId = [aDecoder decodeObjectForKey:@"UserId"];
        self.Name = [aDecoder decodeObjectForKey:@"Name"];
        self.LiveEmail = [aDecoder decodeObjectForKey:@"LiveEmail"];
        self.LiveAuthId = [aDecoder decodeObjectForKey:@"LiveAuthId"];
        self.FBAuthId = [aDecoder decodeObjectForKey:@"FBAuthId"];
        self.CurrentProfileId = [aDecoder decodeObjectForKey:@"CurrentProfileId"];
    }
    return self;
}


@end
