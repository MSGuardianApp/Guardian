
//
//  AppDelegate.m
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "AppDelegate.h"
#import "Reachability.h"
#import<CoreTelephony/CTTelephonyNetworkInfo.h>
#import<CoreTelephony/CTCarrier.h>

#import "NBPhoneMetaDataGenerator.h"

@implementation AppDelegate
@synthesize locationManager = _locationManager;
@synthesize currentLocation =_currentLocation;
@synthesize settingChanged = _settingChanged;
@synthesize arrSubLocations = _arrSubLocations;
@synthesize isSosON = _isSosON;
@synthesize locateBuddyTimer = _locateBuddyTimer;
@synthesize isMigrationFailed = _isMigrationFailed;

//@synthesize kLocationServiceUrl;
//@synthesize kGeoServiceUrl;
//@synthesize kGroupServiceUrl;
//@synthesize kMembershipServiceUrl;
//@synthesize LiveClientID;

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    // Override point for customization after application launch.
    NSSetUncaughtExceptionHandler(&HandleExceptions);
    [[NSNotificationCenter defaultCenter] addObserver: self selector: @selector(reachabilityChanged:) name: kReachabilityChangedNotification object: nil];
    self.isMigrationFailed = false;
    if([[NSUserDefaults standardUserDefaults] objectForKey:@"isMigrationFailed"] == nil){
        [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"isMigrationFailed"];
    }
    
    if([[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"] != nil){
        
            NSString *str = [NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
            NSCharacterSet* notDigits = [[NSCharacterSet decimalDigitCharacterSet] invertedSet];

        if ([str rangeOfCharacterFromSet:notDigits].location == NSNotFound)
        {
//            self.kLocationServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc-dev.cloudapp.net/LocationService.svc/"];
//            self.kGeoServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc-dev.cloudapp.net/GeoUpdate.svc/"];
//            self.kGroupServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc-dev.cloudapp.net/GroupService.svc/"];
//            self.kMembershipServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc-dev.cloudapp.net/MembershipService.svc/"];
//            self.LiveClientID = @"000000004010A627";
//
//            self.kLocationServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/LocationService.svc/"];
//            self.kGeoServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/GeoUpdate.svc/"];
//            self.kGroupServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/GroupService.svc/"];
//            self.kMembershipServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/MembershipService.svc/"];
//            self.LiveClientID = @"0000000044105B2B";
        }
        else{
            [[GlobalClass sharedInstance] migrationUpdate];
//            self.kLocationServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/LocationService.svc/"];
//            self.kGeoServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/GeoUpdate.svc/"];
//            self.kGroupServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/GroupService.svc/"];
//            self.kMembershipServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/MembershipService.svc/"];
//            self.LiveClientID = @"0000000044105B2B";
        }
    }
    else{
//        self.kLocationServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/LocationService.svc/"];
//        self.kGeoServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/GeoUpdate.svc/"];
//        self.kGroupServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/GroupService.svc/"];
//        self.kMembershipServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc.cloudapp.net/MembershipService.svc/"];
//        self.LiveClientID = @"0000000044105B2B";
        //000000004010A627
    }
    
    self.arrLocations = [[NSMutableArray alloc] init];
    self.arrSubLocations = [[NSMutableArray alloc] init];
    appLaunched = YES;
    locCount = 0;
    self.isSosON = NO;

    DBaseInteraction *objDBaseInteraction = [DBaseInteraction sharedInstance];
    [objDBaseInteraction dbManager];
    
    self.window = [[UIWindow alloc] initWithFrame:[[UIScreen mainScreen] bounds]];
    
    if([[GlobalClass sharedInstance] is468SizeScreen]){
        self.HomeVC = [[HomeViewController alloc]initWithNibName:@"HomeViewController" bundle:nil];
    }
    else{
        self.HomeVC = [[HomeViewController alloc]initWithNibName:@"HomeViewController_568" bundle:nil];
    }
    
    [objDBaseInteraction userDataUpdate];
    [objDBaseInteraction userProfileDataUpdate];
    
    _locationManager = [[CLLocationManager alloc] init];
    if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"8.0")) {
        if ([self.locationManager respondsToSelector:@selector(requestAlwaysAuthorization)]) {
            [self.locationManager requestAlwaysAuthorization];
        }
    }
    [_locationManager setDelegate:self];
    [_locationManager setDesiredAccuracy:kCLLocationAccuracyBestForNavigation];
    
    
//    [self performSelector:@selector(locationUpdate) withObject:nil afterDelay:7.0 ];
    
//    [NSTimer scheduledTimerWithTimeInterval:7.5 target: self
//                                   selector: @selector(locationUpdate:) userInfo: nil repeats: YES];
    NSTimer *timer = [NSTimer timerWithTimeInterval:7.5
                                             target:self
                                           selector:@selector(locationUpdate:)
                                           userInfo:nil repeats:YES];
    [[NSRunLoop mainRunLoop] addTimer:timer forMode:NSRunLoopCommonModes];
    
    
//    [NSTimer scheduledTimerWithTimeInterval:30.0 target: self
//                                   selector: @selector(locationUpdateToServer:) userInfo: nil repeats: YES];
    NSTimer *timer1 = [NSTimer timerWithTimeInterval:30.0
                                             target:self
                                           selector:@selector(locationUpdateToServer:)
                                           userInfo:nil repeats:YES];
    [[NSRunLoop mainRunLoop] addTimer:timer1 forMode:NSRunLoopCommonModes];
    
    [self callAfterThirtySeconds:nil];
//    [NSTimer scheduledTimerWithTimeInterval:30.0 target: self
//                                                      selector: @selector(callAfterThirtySeconds:) userInfo: nil repeats: YES];
    NSTimer *timer2 = [NSTimer timerWithTimeInterval:30.0
                                              target:self
                                            selector:@selector(callAfterThirtySeconds:)
                                            userInfo:nil repeats:YES];
    [[NSRunLoop mainRunLoop] addTimer:timer2 forMode:NSRunLoopCommonModes];
    //NSTimer* myTimer =
    
    self.navCont=[[UINavigationController alloc]initWithRootViewController:self.HomeVC];
    self.window.rootViewController = self.navCont;
    self.navCont.navigationBarHidden = YES;
    
    self.window.backgroundColor = [UIColor whiteColor];
    [self.window makeKeyAndVisible];
    return YES;
}

#pragma mark --
#pragma mark Reachibility Methods

- (void) reachabilityChanged: (NSNotification* )note
{
	Reachability* curReach = [note object];
	NSParameterAssert([curReach isKindOfClass: [Reachability class]]);
	
    NetworkStatus netStatus = [curReach currentReachabilityStatus];
    switch (netStatus)
    {
        case ReachableViaWWAN:
        {
            break;
        }
        case ReachableViaWiFi:
        {
            break;
        }
        case NotReachable:
        {
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
            [alert show];
            break;
        }
    }
}

-(void)locationUpdate:(NSTimer *)timer{
    
    
    BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
    if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
        if(servicesEnabled){
            self.locationManager.delegate = self;
            [self.locationManager startUpdatingLocation];
        }
    }
    
    
//    if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
//        if ([CLLocationManager locationServicesEnabled]) {
//            // Find the current location
//            self.locationManager.delegate = self;
//            [self.locationManager startUpdatingLocation];
//            //        [self.locationManager startMonitoringSignificantLocationChanges];
//        }
//        else{
//            if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
//            {
//                UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location Services" message:@"You can enable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
//                [curr1 show];
//            }
//            else
//            {
//                //                   [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
//            }
//        }
//    }
    
}

-(void)locationUpdateToServer:(NSTimer*)t{
    [self postRequestConstruction];
    dispatch_async(dispatch_get_main_queue(), ^{
        [self.HomeVC CurrentLocationIdentifier];
    });
}

-(void) callAfterThirtySeconds:(NSTimer*) t
{
    
    if([[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"])
        {
        if([[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"])
                {
                    if([[GlobalClass sharedInstance] connected]){
                        NSArray *arr = [[DBaseInteraction sharedInstance] getUserTable:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                        
                        if(arr.count>0){
                            NSString *str = [[GlobalClass sharedInstance] dateToTicks:[NSDate date]];
                            NSLog(@"%@",str);
                            NSLog(@"%@",[[arr objectAtIndex:0] objectForKey:@"UserId"]);
                            
                            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:[NSString stringWithFormat:@"%@%@/%@",[NSString stringWithFormat:@"%@",kGetSOSTrackCount],[[arr objectAtIndex:0] objectForKey:@"UserId"],str]] cachePolicy: NSURLRequestUseProtocolCachePolicy
                                                                                timeoutInterval: 60.0];
                            [request1 setHTTPMethod:@"GET"];
                            
                            [request1 setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
                            [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
                            
                            
                            [NSURLConnection sendAsynchronousRequest:request1
                                                               queue:[[NSOperationQueue alloc] init]
                                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                                       if(data){
                                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                                           NSLog(@"%@",object);
                                                           
                                                           dispatch_async(dispatch_get_main_queue(), ^{
                                                               if([object isKindOfClass:[NSArray class]])
                                                                   [self.HomeVC countSOSTrackupdation:object];
                                                           });
                                                       }
                                                   }];
                        }

                    }
                    else{
//                        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//                        [alert show];
                    }
                }
        }
    
    
}
//[self.locationManager startMonitoringSignificantLocationChanges]

#pragma mark CLLocation Delegate Methods
#pragma mark

- (void)locationManager:(CLLocationManager *)manager didUpdateLocations:(NSArray *)locations
{
    // locations contains an array of recent locations, but this app only cares about the most recent
    // which is also "manager.location"
    
//    if(locCount > 7){
    
    
        if(locations.count>0)
            [self locationmanaging:locations];
//        CLLocation *newLocation = locations.lastObject;
//        
////        NSTimeInterval locationAge = -[newLocation.timestamp timeIntervalSinceNow];
////        if (locationAge > 5.0) return;
//        
//        if (newLocation.horizontalAccuracy < 0) return;
//        
//                if(self.arrLocations.count>1){
//                    CLLocation *oldLocation = self.arrLocations.lastObject;
//                    
//                    double distance = [newLocation distanceFromLocation:oldLocation];
//                    
//                    if(distance>10){
//                        [self locationmanaging:locations];
//                    }
//                }
//                else{
//                    [self locationmanaging:locations];
//                }
        
//        }
//    else{
//        locCount++;
//    }
    
}



-(void)locationmanaging:(NSArray *)locations{
    locCount = 0;
    dispatch_async(dispatch_get_main_queue(), ^{
        self.locationManager.delegate = nil;
        [self.locationManager stopUpdatingLocation];
        
        if(locations.count>0){
        
            CLLocation *loc = [locations lastObject];
            
            
            if(loc.horizontalAccuracy>0){
                
                //[[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]
                NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
                if( [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"] || [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy] == NULL){
                    if([[[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy] count]==0){
                        [self foundLocation:loc];
                        return ;
                    }
                }
                
//                [self foundLocation:loc];
                
                if(loc.horizontalAccuracy<150){
                    if(prevLoc){
                        double distance = [loc distanceFromLocation:prevLoc];
                        if(distance>25)
                            [self foundLocation:loc];
                    }
                    else
                        [self foundLocation:loc];
                    
                }
                else{
                    
                    if(capturedTime == nil || capturedTime == NULL){
                        capturedTime = [NSDate date];
                    }
                    else{
                        NSTimeInterval distanceBetweenDates = [[NSDate date] timeIntervalSinceDate:capturedTime];
                        double minutes = distanceBetweenDates/60;
                        
                        
                        if(minutes>5){
                            [self foundLocation:loc];
//                            if(prevLoc){
//                                double distance = [loc distanceFromLocation:prevLoc];
//                                if(distance>25)
//                                    [self foundLocation:loc];
//                            }
//                            else
//                                [self foundLocation:loc];
                        }
                    }
                    
                }

            }
        }
//        [self performSelector:@selector(locationUpdate) withObject:nil afterDelay:7.0 ];
    });
    
}
- (void)locationManager:(CLLocationManager *)manager didFailWithError:(NSError *)error
{
    NSLog(@"Location manager failed with error: %@", error);
    if ([error.domain isEqualToString:kCLErrorDomain] && error.code == kCLErrorDenied) {
        //user denied location services so stop updating manager
        [manager stopUpdatingLocation];
        //respect user privacy and remove stored location
    }
}

-(void)stopPosting{
    if([[GlobalClass sharedInstance] connected]){
        NSString *authId;
        if([[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"])
            authId = [[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"];
        else authId =@"";
        
        
        NSString *urlString = [NSString stringWithFormat:@"%@%@/%@/%@",[NSString stringWithFormat:@"%@",kStopPostingsUrl],[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],[[GlobalClass sharedInstance] getSessiontoken],[[GlobalClass sharedInstance] dateToTicks:[NSDate date]]];
        
        NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
        [request1 setHTTPMethod:@"GET"];
        [request1 setValue:authId forHTTPHeaderField:@"AuthID"];
        [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
        
        [NSURLConnection sendAsynchronousRequest:request1
                                           queue:[[NSOperationQueue alloc] init]
                               completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                   if(data){
                                       NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                       NSLog(@"%@",jsonString);
                                       if(!error){
                                           
                                           NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                           NSLog(@"%@",jsonString);
                                           
                                           
                                           
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               [self postRequestConstruction];
                                           });
                                       }
                                       else{
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                           });
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
                                       }

                                   }
                               }];
    }
    else{
//        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//        [alert show];
    }
    
}


-(void)updatingLocationsToServer:(NSArray *)locations{
//    startPushpin = NO;
//    AppDelegate* appDel = (AppDelegate*)[[UIApplication sharedApplication] delegate];
    
    if(appLaunched){
        
        [self postRequestConstruction];
        appLaunched = NO;
        return;
    }
    
    
    if(_currentLocation!=nil){
        CLLocation *newLocation = locations.lastObject;
        
        NSTimeInterval locationAge = -[newLocation.timestamp timeIntervalSinceNow];
        if (locationAge > 5.0) return;
        
        if (newLocation.horizontalAccuracy < 0) return;
        
        // Needed to filter cached and too old locations
        //NSLog(@"Location updated to = %@", newLocation);
        CLLocation *loc1 = [[CLLocation alloc] initWithLatitude:_currentLocation.coordinate.latitude longitude:_currentLocation.coordinate.longitude];
        CLLocation *loc2 = [[CLLocation alloc] initWithLatitude:newLocation.coordinate.latitude longitude:newLocation.coordinate.longitude];
        double distance = [loc1 distanceFromLocation:loc2];
        _currentLocation = newLocation;
        
        if(distance > 100)
        {
            //significant location update
            [self postRequestConstruction];
        }
    }
    else{
        
        [self postRequestConstruction];
        _currentLocation = locations.lastObject;
    }
}

-(void)postRequestConstruction{
    
    if([[[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] boolValue] || [[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
        
        if([[NSUserDefaults standardUserDefaults] boolForKey:@"PostLocationConsent"]){
            if(self.arrLocations.count>0){
                
                if([[GlobalClass sharedInstance] connected]){
                    
                    NSMutableArray *arrLoc  = [[NSMutableArray alloc] init];
                    NSMutableArray *latArr = [[NSMutableArray alloc] init];
                    NSMutableArray *longArr = [[NSMutableArray alloc] init];
                    NSMutableArray *SpdArr = [[NSMutableArray alloc] init];
                    NSMutableArray *AltArr = [[NSMutableArray alloc] init];
                    NSMutableArray *AccArr = [[NSMutableArray alloc] init];
                    NSMutableArray *TSArr = [[NSMutableArray alloc] init];
                    NSMutableArray *MsgTypeArr = [[NSMutableArray alloc] init];
                    
                    for (int i=0; i< self.arrSubLocations.count; i++) {
                        
                        GeoTag *obj = (GeoTag *)[self.arrSubLocations objectAtIndex:i];
                        [latArr addObject:obj.Lati];
                        [longArr addObject:obj.Longi];
                        [SpdArr addObject:obj.Speed];
                        [AltArr addObject:obj.Altitude];
                        [AccArr addObject:obj.accuracy];
                        [MsgTypeArr addObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"TokenId"]]];
                        //                NSLog(@"%@",[[GlobalClass sharedInstance] dateToTicks:loc.timestamp]);
                        
                        NSNumberFormatter * f = [[NSNumberFormatter alloc] init];
                        [f setNumberStyle:NSNumberFormatterDecimalStyle];
                        NSNumber * myNumber = [f numberFromString:obj.timeStamp];
                        
                        [TSArr addObject:myNumber];
                    }
                    
                    if(self.arrSubLocations.count>0){
                        
                        NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
                        
                        [dict setObject:latArr forKey:@"Lat"];
                        [dict setObject:longArr forKey:@"Long"];
                        [dict setObject:SpdArr forKey:@"Spd"];
                        [dict setObject:AltArr forKey:@"Alt"];
                        [dict setObject:AccArr forKey:@"Accuracy"];
                        [dict setObject:TSArr forKey:@"TS"];
                        
                        
                        //,[[NSUserDefaults standardUserDefaults] objectForKey:@"TokenId"]
                        
                        [dict setObject:[NSNumber numberWithInteger:self.arrSubLocations.count] forKey:@"LocCnt"];
                        [dict setObject:MsgTypeArr forKey:@"IsSOS"];
                        [dict setObject:[NSString stringWithFormat:@",0,"] forKey:@"GroupID"];
                        [dict setObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]] forKey:@"PID"];
                        
//                        if(self.isStartPushPin){
//                            [dict setObject:[NSString stringWithFormat:@"B"] forKey:@"Cmd"];
//                            self.isStartPushPin =NO;
//                        }
//                        else{
//                            [dict setObject:[NSString stringWithFormat:@"E"] forKey:@"Cmd"];
//                        }
                        
//                        if([[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
//                            [dict setObject:[NSString stringWithFormat:@"B"] forKey:@"Cmd"];
//                        }
//                        else {
//                            [dict setObject:[NSString stringWithFormat:@"E"] forKey:@"Cmd"];
//                        }
                        
                        
                        if([[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"] ){
                            [dict setObject:[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"] forKey:@"Id"];
                        }
                        else{
                            [dict setObject:[NSString stringWithFormat:@"0"] forKey:@"Id"];
                        }
                        [dict setObject:[NSString stringWithFormat:@"%lu",(unsigned long)self.arrSubLocations.count] forKey:@"LocCnt"];
                        
                        [arrLoc addObject:dict];
                        
                        [self postLocationsInBackground:arrLoc andIndex:0];
                    }
                }
                
                else{
                    //                    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
                    //                    [alert show];
                }
            }
            
        }
    }
}


-(void)postLocationsInBackground:(NSMutableArray *)arr andIndex:(NSInteger)NoOfTimes {
    if(NoOfTimes<3){
        if(arr.count>0){
            NSError* error = nil;
            NSData *data = [NSJSONSerialization dataWithJSONObject:[arr objectAtIndex:0]  options:0 error:&error];
            NSString *requestStr = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
            
            NSString *authId;
            if([[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"])
                 authId = [[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"];
            else authId =@"";
            
            
            NSString *urlString = [NSString stringWithFormat:@"%@",[NSString stringWithFormat:@"%@",kPostMyLocationUrl]];
            
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
            [request1 setHTTPMethod:@"POST"];
            [request1 setValue:authId forHTTPHeaderField:@"AuthID"];
            [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
            [request1 setValue: @"application/json" forHTTPHeaderField: @"Content-Type"];
            [request1 setHTTPBody:[requestStr dataUsingEncoding:NSUTF8StringEncoding]];
            
            [NSURLConnection sendAsynchronousRequest:request1
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                           if(!error && data){
                                               
                                               NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                               NSLog(@"%@",jsonString);
                                               [self.arrSubLocations removeAllObjects];
                                               
                                               id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                               NSLog(@"%@",object);
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   
                                               });
                                           }
                                           else{
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   [self postLocationsInBackground:arr andIndex:(NoOfTimes+1)];
                                               });
                                           }
                                   }];
        }
    }
    
}

- (void)foundLocation:(CLLocation *)location
{
    @try {
        NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
        if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
            self.arrLocations = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
        }
        
        if(location){
            
//            NSInteger prevSpeed = -1;
//            NSInteger speed = 0;
//            if(self.arrLocations.count>0){
//                GeoTag *obj = (GeoTag *)[self.arrLocations lastObject];
//                if([obj.Speed integerValue] > 0){
//                    prevSpeed = [obj.Speed  integerValue];
//                }
//                speed = (location.speed-prevSpeed);
//            }
            
                GeoTag *obj = [[GeoTag alloc] init];
                obj.Lati = [NSString stringWithFormat:@"%f",location.coordinate.latitude];
                obj.Longi = [NSString stringWithFormat:@"%f",location.coordinate.longitude];
                obj.Speed = [NSString stringWithFormat:@"%@",[NSNumber numberWithInteger:location.speed]];
                obj.Altitude = [NSString stringWithFormat:@"%@",[NSNumber numberWithInt:location.altitude]];
                obj.timeStamp= [[GlobalClass sharedInstance] dateToTicks:location.timestamp];
                obj.accuracy = [NSString stringWithFormat:@"%@",[NSNumber numberWithDouble:location.horizontalAccuracy]];
                NSString *str = @"";
                
                if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
                    
                    if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] boolValue]){
                        str = @"2";
                    }
                    else{
                        str= @"0";
                    }
                }
                else{
                    str= @"1";
                }
                obj.status = str;
                
                capturedTime = [NSDate date];
                prevLoc = location;
                [self.arrLocations addObject:obj];
                [self.arrSubLocations addObject:obj];
            
            
            //        if(self.arrLocations.count<11)
            //            [self.arrLocations addObject:location];
            //        else{
            //            [self.arrLocations removeObjectAtIndex:0];
            //            [self.arrLocations mutableCopy];
            //            [self.arrLocations addObject:location];
            //        }
        }
        
        
        NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
        NSData *dataSave = [NSKeyedArchiver archivedDataWithRootObject:self.arrLocations];
        [userDefaults setObject:dataSave forKey:@"Locations"];
        [userDefaults synchronize];
        
        [[NSNotificationCenter defaultCenter] postNotificationName:@"UserLocationUpdate" object:nil];
        
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}


- (BOOL)application:(UIApplication *)application
            openURL:(NSURL *)url
  sourceApplication:(NSString *)sourceApplication
         annotation:(id)annotation
{
    //486681428059978
//    if (url != nil && [[url scheme] isEqualToString:@"fb486681428059978"])
//    {
//        return [FBAppCall handleOpenURL:url
//                      sourceApplication:sourceApplication
//                            withSession:self.session];
//    }
    return YES;
}


-(void) createLocalRepeatedNotification:(NSString *)notifyType
{
    
    UILocalNotification *localNotification = [[UILocalNotification alloc] init];
    NSTimeInterval your_custom_fire_interval = 60; // interval in seconds
    NSDate *remindDate = [[NSDate date] dateByAddingTimeInterval:your_custom_fire_interval];
    localNotification.fireDate = remindDate;
    localNotification.repeatInterval = NSCalendarUnitMinute;
    localNotification.userInfo = @{@"period": [NSNumber numberWithInteger:your_custom_fire_interval]};
    NSString *notifyBody = @"";
    if([notifyType isEqualToString:@"SOS"]){
        notifyBody = [NSString stringWithFormat:@"If you are safe, please turn SOS off."];
    }
    else{
        notifyBody = [NSString stringWithFormat:@"To save battery, please turn off tracking."];
    }
    
    localNotification.alertBody = notifyBody;
    [[UIApplication sharedApplication] scheduleLocalNotification:localNotification];
}


- (void)applicationWillResignActive:(UIApplication *)application
{
    // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
    // Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
}



- (void)applicationDidEnterBackground:(UIApplication *)application
{
//    [self.locationManager stopUpdatingLocation];
//    [self.locationManager startUpdatingLocation];
    
    
    
    if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
        if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] boolValue]){
        }
        else{
            [self createLocalRepeatedNotification:@"TrackMe"];
        }
    }
    else{
        [self createLocalRepeatedNotification:@"SOS"];
    }
	
    if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsRatedApp"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsRatedApp"] boolValue]){
        UIAlertView *alert =  [[UIAlertView alloc] initWithTitle:@"Rate Guardian" message:@"If you liked the app, Please rate and review it on the store to help us to improve it" delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"Ok", nil];
        alert.tag = 100;
        [alert show];
    }
    // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later. 
    // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
}


- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex{
    if(alertView.tag==100){
        @try {
            if(buttonIndex == 1){
                
                [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"itms://itunes.apple.com/in/app/guardianapp/id979153515?mt=8"]];
                [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsRatedApp"];
            }
            else{
            }
            NSLog(@"%ld",(long)buttonIndex);
        }
        @catch (NSException *exception) {
            [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
            NSLog(@"%@",exception);
        }
        @finally {
        }
    }
}

- (void)applicationWillEnterForeground:(UIApplication *)application
{
    // Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
}

- (void)applicationDidBecomeActive:(UIApplication *)application
{
    [[UIApplication sharedApplication] cancelAllLocalNotifications];
        // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
        
//        [FBAppEvents activateApp];
    // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
}

- (void)applicationWillTerminate:(UIApplication *)application
{
    // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
}

void HandleExceptions(NSException *exception) {
    
    UIAlertView *alrt = [[UIAlertView alloc] initWithTitle:@"Test" message:[NSString stringWithFormat:@"The app has encountered an unhandled exception: %@",[exception debugDescription]] delegate:nil cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil];
    [alrt show];
    NSLog(@"The app has encountered an unhandled exception: %@", [exception debugDescription]);
    // Save application data on crash
}

@end

