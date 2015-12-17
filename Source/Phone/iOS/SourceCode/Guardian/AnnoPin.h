//
//  AnnoPin.h
//  Guardian
//
//  Created by PTG on 18/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Mapkit/Mapkit.h"

@interface AnnoPin : NSObject <MKAnnotation>
{
    CLLocationCoordinate2D coordinate;
    NSString *stCode;
    NSInteger nTag;
    NSString *title;
}
@property (nonatomic, readonly) CLLocationCoordinate2D coordinate;
-(id)initWithCordinatep:(CLLocationCoordinate2D ) dict;
@property(nonatomic,retain)NSString *stCode;
@property (nonatomic) NSInteger nTag;
@property (nonatomic,copy) NSString *title;

@end