//
//  GeoTag.h
//  Guardian
//
//  Created by PTG on 04/03/15.
//  Copyright (c) 2015 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface GeoTag : NSObject
@property(nonatomic,strong)NSString *Lati;
@property(nonatomic,strong)NSString *Longi;
@property(nonatomic,strong)NSString *status;
@property(nonatomic,strong)NSString *Speed;
@property(nonatomic,strong)NSString *Altitude;
@property(nonatomic,strong)NSString *timeStamp;
@property(nonatomic,strong)NSString *accuracy;
@end
