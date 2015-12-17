//
//  AppDelegate.h
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "HomeViewController.h"
#import <CoreLocation/CoreLocation.h>

@class Reachability;

@interface AppDelegate : UIResponder <UIApplicationDelegate,CLLocationManagerDelegate>{
    Reachability *internetReach;
    BOOL appLaunched;
    int locCount;
    BOOL settingChanged;
    BOOL isSosON;
    NSDate *capturedTime;
    CLLocation *prevLoc;
}

@property (strong, nonatomic) UIWindow *window;
@property(nonatomic,strong)UINavigationController *navCont;
@property(nonatomic,strong)HomeViewController *HomeVC;
@property (strong , nonatomic) CLLocationManager *locationManager;
@property (strong , nonatomic) NSMutableArray *arrLocations;
@property (strong , nonatomic) NSMutableArray *arrSubLocations;
@property (strong , nonatomic) CLLocation *currentLocation;
@property (assign, nonatomic) BOOL settingChanged;
@property (assign, nonatomic) BOOL isSosON;
@property (retain , nonatomic) NSTimer *locateBuddyTimer;
@property (assign, nonatomic) BOOL isMigrationFailed;
@property (assign, nonatomic) BOOL isStartPushPin;


//@property (retain , nonatomic) NSString *kLocationServiceUrl;
//@property (retain , nonatomic) NSString *kGeoServiceUrl;
//@property (retain , nonatomic) NSString *kGroupServiceUrl;
//@property (retain , nonatomic) NSString *kMembershipServiceUrl;
//@property (retain , nonatomic) NSString *LiveClientID;

-(void)postRequestConstruction;
- (void)foundLocation:(CLLocation *)location;
@end
