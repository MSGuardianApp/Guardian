//
//  RouteMapViewController.h
//  Guardian
//
//  Created by PTG on 02/02/15.
//  Copyright (c) 2015 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <MapKit/MapKit.h>
#import "AnnoPin.h"

@interface RouteMapViewController : UIViewController{
    CLLocation *loc;
    NSMutableArray *arrRoutePoints;
    CLLocationCoordinate2D coordinateUpto;
    MKPolyline *polyline;
    MKRoute *routeDetails;
    BOOL isGPSTapped;
    NSString *timeStamper;
    NSTimer *buddyLocateTimer;
}
@property (nonatomic,retain) MKPolyline *polyline;
@property (nonatomic,retain) CLLocation *locA;
@property (nonatomic,retain) NSString *IsBuddySOS;
@property (nonatomic,retain) NSString *selPhoneNumber;
@property (nonatomic,retain) NSString *buddyprofileId;
@property (nonatomic,retain) NSMutableDictionary *buddyData;

@end
