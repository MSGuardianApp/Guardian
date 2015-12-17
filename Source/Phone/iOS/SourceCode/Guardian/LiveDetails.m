//
//  LiveDetails.m
//  Guardian
//
//  Created by PTG on 24/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "LiveDetails.h"

@implementation LiveDetails



- (void)encodeWithCoder:(NSCoder *)encoder
{
    [encoder encodeObject:self.authenticationToken forKey:@"authenticationToken"];
    [encoder encodeObject:self.refreshToken forKey:@"refreshToken"];
    [encoder encodeObject:self.accessToken forKey:@"accessToken"];
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    if(self = [super init])
    {
        self.authenticationToken = [aDecoder decodeObjectForKey:@"authenticationToken"];
        self.refreshToken = [aDecoder decodeObjectForKey:@"refreshToken"];
        self.accessToken = [aDecoder decodeObjectForKey:@"accessToken"];
    }
    return self;
}

@end
