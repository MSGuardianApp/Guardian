//
//  AnnoPin.m
//  Guardian
//
//  Created by PTG on 18/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "AnnoPin.h"

@implementation AnnoPin

@synthesize coordinate,stCode;
@synthesize nTag;
@synthesize title;
-(id)initWithCordinatep:(CLLocationCoordinate2D ) dict{
    CLLocationCoordinate2D center;
    center.latitude=dict.latitude;
    center.longitude=dict.longitude;
    coordinate=center;
    return self;
}

@end
