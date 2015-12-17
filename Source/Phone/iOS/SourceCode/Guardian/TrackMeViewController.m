//
//  TrackMeViewController.m
//  Guardian
//
//  Created by PTG on 02/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "TrackMeViewController.h"
#import "AnnoPin.h"
#import "SearchLocationCustomCell.h"
#import "SOSHostViewController.h"
#import <MapKit/MapKit.h>
@interface TrackMeViewController ()
@property (nonatomic , weak) IBOutlet MKMapView *objMapView;
@property (nonatomic , weak) IBOutlet UISearchBar *searchBar;
@property (nonatomic , retain) IBOutlet UITableView *tbleView;
@property (nonatomic , retain) IBOutlet UIView *searchView;
@property (nonatomic , retain) IBOutlet UILabel *lblTitle;
@property (nonatomic , weak) IBOutlet UIButton *btnStopSOS;
@property (nonatomic , weak) IBOutlet UIButton *btnStopTracking;
@property (nonatomic , retain) IBOutlet UILabel *lblStartSOS;
@property (nonatomic , retain) IBOutlet UILabel *lblStartTrack;
@property (nonatomic , retain) IBOutlet UILabel *lblFocus;
@property (nonatomic , retain) IBOutlet UILabel *lblShowRoute;
@property (nonatomic , retain) IBOutlet UILabel *lblSpeed;
@property (nonatomic , retain) IBOutlet UILabel *lblAccuracy;

@property (nonatomic , weak) IBOutlet UIWebView *webview;

-(IBAction)btnBackClicked:(id)sender;

-(IBAction)sosBtnCicked:(UIButton *)sender;
-(IBAction)trackMeBtnCicked:(UIButton *)sender;
-(IBAction)focusMeBtnCicked:(UIButton *)sender;
-(IBAction)routeBtnCicked:(UIButton *)sender;

@end

@implementation TrackMeViewController
@synthesize polyline;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    
    
    
    arrRoutePoints = [[NSMutableArray alloc] init];
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arrRoutePoints = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    loc = (GeoTag *)[arrRoutePoints lastObject];
    
    
    
    self.lblSpeed.text = [NSString stringWithFormat:@"Speed :%@",loc.Speed];
    self.lblAccuracy.text = [NSString stringWithFormat:@"Accuracy :%@",loc.accuracy];
    
    searchPoints = [[NSMutableArray alloc] init];
//    self.lpgr = [[UILongPressGestureRecognizer alloc] initWithTarget:self action:@selector(handleLongPressGestures:)];
//    self.lpgr.minimumPressDuration = 1.0f;
//    self.lpgr.allowableMovement = 100.0f;
//    
//    [self.objMapView addGestureRecognizer:self.lpgr];
    
    [self.searchView setFrame:CGRectMake(10,105, 300,[UIScreen mainScreen].applicationFrame.size.height-150)];
    
    isTrackingRoute = NO;
    
    destValueAssigned = NO;
    isRouteEnabled = NO;
    self.tbleView.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    self.lblTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblTitle.font.pointSize];
    
    [[NSNotificationCenter defaultCenter] removeObserver:self];
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(userLocationChanged:)
                                                 name:@"UserLocationUpdate"
                                               object:nil];
    
    [self setfontForlabels];
    // Do any additional setup after loading the view from its nib.
}

-(void)setfontForlabels {
    
    self.lblTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblTitle.font.pointSize];
    self.lblStartSOS.font = [UIFont fontWithName:@"SegoeUI" size:self.lblStartSOS.font.pointSize];
    self.lblStartTrack.font = [UIFont fontWithName:@"SegoeUI" size:self.lblStartTrack.font.pointSize];
    self.lblFocus.font = [UIFont fontWithName:@"SegoeUI" size:self.lblFocus.font.pointSize];
    self.lblShowRoute.font = [UIFont fontWithName:@"SegoeUI" size:self.lblShowRoute.font.pointSize];
    self.lblSpeed.font = [UIFont fontWithName:@"SegoeUI" size:self.lblSpeed.font.pointSize];
    self.lblAccuracy.font = [UIFont fontWithName:@"SegoeUI" size:self.lblAccuracy.font.pointSize];
}



-(void) viewWillAppear:(BOOL)animated
{
	[super viewWillAppear:animated];
    
    NSLog(@"%@",self.webview);
    self.webview.delegate = self;
    //[NSURL fileURLWithPath:[[NSBundle mainBundle]pathForResource:@"map" ofType:@"html"]isDirectory:NO]
    [self.webview loadRequest:[NSURLRequest requestWithURL:[NSURL fileURLWithPath:[[NSBundle mainBundle]pathForResource:@"map" ofType:@"html"]isDirectory:NO]]];
    
//    @try {
//        NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
//        if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
//            arrRoutePoints = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
//        }
//        
//        if(arrRoutePoints.count>0){
//            
//            loc = [arrRoutePoints lastObject];
//            
//            AnnoPin *pin = [[AnnoPin alloc]initWithCordinatep:CLLocationCoordinate2DMake(loc.coordinate.latitude,loc.coordinate.longitude)];
//            pin.nTag = 1;
//            pin.title =@"IN";
//            [self.objMapView addAnnotation:pin];
//            
//            //Get your location and create a CLLocation
//            MKCoordinateRegion region; //create a region.  No this is not a pointer
//            region.center = loc.coordinate;  // set the region center to your current location
//            MKCoordinateSpan span; // create a range of your view
//            span.latitudeDelta =37.7749300*5/2 ;  // span dimensions.  I have BASE_RADIUS defined as 0.0144927536 which is equivalent to 1 mile
//            span.longitudeDelta =-122.4194200*5/2;  // span dimensions
//            region.span = span; // Set the region's span to the new span.
//            
//            span = MKCoordinateSpanMake(0, 360/pow(2, 15)*self.objMapView.frame.size.width/256);
//            [self.objMapView setRegion:MKCoordinateRegionMake(region.center, span) animated:YES];
//            
//            
//            if(arrRoutePoints.count>1){
//                
//                
//                CLLocation *locat = [arrRoutePoints objectAtIndex:0];
//                
//                
//                AnnoPin *pin = [[AnnoPin alloc]initWithCordinatep:CLLocationCoordinate2DMake(locat.coordinate.latitude,locat.coordinate.longitude)];
//                pin.nTag = 2;
//                pin.title =@"END";
//                [self.objMapView addAnnotation:pin];
//                
//                // remove polyline if one exists
//                [self.objMapView removeOverlay:self.polyline];
//                
//                // create an array of coordinates from allPins
//                CLLocationCoordinate2D coordinates[arrRoutePoints.count];
//                int i = 0;
//                for (CLLocation *locations in arrRoutePoints) {
//                    coordinates[i] = locations.coordinate;
//                    i++;
//                }
//                isTrackingRoute = YES;
//                // create a polyline with all cooridnates
//                //            MKPolyline *polylin = [MKPolyline polylineWithCoordinates:coordinates count:arrRoutePoints.count];
//                //            [self.objMapView addOverlay:polylin level:MKOverlayLevelAboveRoads];
//                //            self.polyline = polylin;
//                
//                [self getDirectionsfrom:loc.coordinate andTo:locat.coordinate];
//            }
//        }
//    }
//    @catch (NSException *exception) {
//        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
//        NSLog(@"%@",exception);
//    }
//    @finally {
//        
//    }
    
    
    
    
//    AppDelegate* appDel = (AppDelegate*)[[UIApplication sharedApplication] delegate];
//    if(appDel.arrLocations.count>0){
//        NSInteger lastIndex = [appDel.arrLocations count] - 1;
//        
//        loc = [appDel.arrLocations objectAtIndex:lastIndex];
//        
//        
//        //Get your location and create a CLLocation
//        MKCoordinateRegion region; //create a region.  No this is not a pointer
//        region.center = loc.coordinate;  // set the region center to your current location
//        MKCoordinateSpan span; // create a range of your view
//        span.latitudeDelta =37.7749300*5/2 ;  // span dimensions.  I have BASE_RADIUS defined as 0.0144927536 which is equivalent to 1 mile
//        span.longitudeDelta =-122.4194200*5/2;  // span dimensions
//        region.span = span; // Set the region's span to the new span.
//        
//        span = MKCoordinateSpanMake(0, 360/pow(2, 15)*self.objMapView.frame.size.width/256);
//        [self.objMapView setRegion:MKCoordinateRegionMake(region.center, span) animated:YES];
//        
//        CLLocation *newLocation = [[CLLocation alloc] initWithLatitude:loc.coordinate.latitude longitude:loc.coordinate.longitude];
//        MKPointAnnotation *annotation = [[MKPointAnnotation alloc] init];
//        annotation.coordinate = CLLocationCoordinate2DMake(newLocation.coordinate.latitude, newLocation.coordinate.longitude);
//        annotation.title = @"You";
//        [self.objMapView addAnnotation:annotation];
//        
//    }
}

-(void)viewDidAppear:(BOOL)animated{
    [super viewDidAppear:YES];
    
    if([[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] && [[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
        self.btnStopSOS.selected = YES;
        self.lblStartSOS.text = @"Stop SOS";
    }
    else{
        self.btnStopSOS.selected = NO;
        self.lblStartSOS.text = @"Strat SOS";
    }
    
    if([[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] && [[[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] boolValue]){
        self.btnStopTracking.selected = YES;
        self.lblStartTrack.text = @"Stop Tracking";
    }
    else{
        self.btnStopTracking.selected = NO;
        self.lblStartTrack.text = @"Start Tracking";
    }
}

-(void)userLocationChanged:(NSNotification *)notification{
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arrRoutePoints = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    if(arrRoutePoints.count>0){
        GeoTag *loc1 = (GeoTag *)[arrRoutePoints lastObject];
        NSString *str= [NSString stringWithFormat:@"appendPathToExisting('%@','%@','%@')",loc1.Lati,loc1.Longi,loc1.status];
        self.lblSpeed.text = [NSString stringWithFormat:@"Speed :%@",loc1.Speed];
        self.lblAccuracy.text = [NSString stringWithFormat:@"Accuracy :%@",loc1.accuracy];
        [self.webview stringByEvaluatingJavaScriptFromString:str];
    }
}


-(IBAction)sosBtnCicked:(UIButton *)sender{
    @try {
        if(sender.selected) {
            
            self.btnStopSOS.selected = NO;
            self.lblStartSOS.text = @"Strat SOS";
            [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"IsSosON"];
            [[NSUserDefaults standardUserDefaults] synchronize];
            [[DBaseInteraction sharedInstance] updateSOS:NO forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
            [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
            [UIApplication sharedApplication].idleTimerDisabled = NO;
            [[GlobalClass sharedInstance] stopSOSandIndex:0];
            
            NSArray *arrBuddies = [[DBaseInteraction sharedInstance] getBuddyPhoneNumbers] ;
            NSMutableArray *arrPhone ;
            if(!arrPhone)
                arrPhone = [[NSMutableArray alloc] init];
            else{
                [arrPhone removeAllObjects];
                [arrPhone mutableCopy];
            }
            
            for (int i=0; i<arrBuddies.count; i++) {
                [arrPhone addObject:[[arrBuddies objectAtIndex:i] objectForKey:@"PhoneNumber"]];
            }
            
            [self showSMS:[NSString stringWithFormat:@"I'm safe now. Thanks for the help! I will get in touch with you shortly and share the details."] ForRecipents:arrPhone];
        }
        else{
            SOSHostViewController *ObjSOSHostViewController = [[SOSHostViewController alloc] init];
            [self.navigationController pushViewController:ObjSOSHostViewController animated:NO];
            //                self.btnStopSOS.selected = YES;
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}

-(IBAction)captureBtnCicked:(UIButton *)sender{
    @try {
        if(self.btnStopTracking.selected){
            
            if ([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera])
            {
                UIImagePickerController *imagePicker = [[UIImagePickerController alloc]init];
                imagePicker.delegate = self;
                imagePicker.sourceType = UIImagePickerControllerSourceTypeCamera;
                imagePicker.allowsEditing = YES;
                
                [self presentViewController:imagePicker animated:YES completion:nil];
            }else{
                UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Camera Unavailable"
                                                               message:@"Unable to find a camera on your device."
                                                              delegate:nil
                                                     cancelButtonTitle:@"OK"
                                                     otherButtonTitles:nil, nil];
                [alert show];
                alert = nil;
            }
            
        }
        else{
            
            UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Guardian"
                                                           message:@"Please check your SOS is OFF."
                                                          delegate:nil
                                                 cancelButtonTitle:@"OK"
                                                 otherButtonTitles:nil, nil];
            [alert show];
            alert = nil;
        }
        
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}

-(IBAction)trackMeBtnCicked:(UIButton *)sender{
    @try {
        if(self.btnStopTracking.selected) {
            
            
            if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
                self.btnStopSOS.selected = NO;
                self.btnStopTracking.selected = NO;
                self.lblStartTrack.text = @"Start Tracking";
                [UIApplication sharedApplication].idleTimerDisabled = NO;
                [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"IsTrackingON"];
                [[GlobalClass sharedInstance] stopPostingandIndex:0];
                [[NSUserDefaults standardUserDefaults] setObject:@"" forKey:@"SessionToken"];
                [[DBaseInteraction sharedInstance] updateSessionToken:@"" andTracking:NO forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
                AppDelegate* appDel = (AppDelegate*)[[UIApplication sharedApplication] delegate];
                [appDel.arrLocations removeAllObjects];
                
                NSMutableArray *arr = [[NSMutableArray alloc] init];
                NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
                NSData *dataSave = [NSKeyedArchiver archivedDataWithRootObject:arr];
                [userDefaults setObject:dataSave forKey:@"Locations"];
                [userDefaults synchronize];
            }
            else{
                UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Guardian"
                                                               message:@"Please check your SOS is ON."
                                                              delegate:nil
                                                     cancelButtonTitle:@"OK"
                                                     otherButtonTitles:nil, nil];
                [alert show];
                alert = nil;
                
            }
        }
        else
        {
            
            BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
            if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
                if(!servicesEnabled){
                    if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
                    {
                        UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location Services" message:@"You can enable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
                        [curr1 show];
                    }
                    else
                    {
                        [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
                    }
                }
                else{
                    self.btnStopTracking.selected = YES;
                    self.lblStartTrack.text = @"Stop Tracking";
                    GlobalClass *objGlobalClass = [[GlobalClass alloc] init];
                    [objGlobalClass stopPostingandIndex:0];
                    [UIApplication sharedApplication].idleTimerDisabled = YES;
                    [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsTrackingON"];
                    [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
                    NSString* stringUUID = [[NSUUID UUID] UUIDString];
                    [[NSUserDefaults standardUserDefaults] setObject:stringUUID forKey:@"SessionToken"];
                    [self postLocationArray];
                    [[DBaseInteraction sharedInstance] updateSessionToken:stringUUID andTracking:YES forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                }
            }
            else{
                
                UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Location consent is disabled in Preferences.Do you want to enable ?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes", nil];
                curr1.tag =100;
                [curr1 show];
                
            }
            
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}

- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex{
    if(alertView.tag==100){
        @try {
            if(buttonIndex == 1){
                [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"LocationConsent"];;
                [[DBaseInteraction sharedInstance]  updatetLocationConsent:YES forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                //                HostViewController *obj = [[HostViewController alloc] init];
                //                [self.navigationController pushViewController:obj animated:NO];
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
-(IBAction)focusMeBtnCicked:(UIButton *)sender{
    if(arrRoutePoints.count>0){
        GeoTag *loc1 = (GeoTag *)[arrRoutePoints lastObject];
        NSString *str= [NSString stringWithFormat:@"takeMeToMyPlace('%@','%@')",loc1.Lati,loc1.Longi];
        [self.webview stringByEvaluatingJavaScriptFromString:str];
    }
}


- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType {
    
    NSString *triggerString=[[request URL] absoluteString];
    if([triggerString isEqualToString:@"ios:mapLoaded"]) {
        NSString *strLoc = @"";
        for(int i=0;i<[arrRoutePoints count];i++){
            GeoTag *loca = (GeoTag *)[arrRoutePoints objectAtIndex:i];
            strLoc = [strLoc stringByAppendingString:[NSString stringWithFormat:@"%@-%@-%@",loca.Lati,loca.Longi,loca.status]];
            if(i<[arrRoutePoints count]-1){
                strLoc = [strLoc stringByAppendingString:@","];
            }
        }
        GeoTag *loc1 = (GeoTag *)[arrRoutePoints lastObject];
        NSString *str= [NSString stringWithFormat:@"createRouteToDestLocArray('%@','%@','%@','Track')",loc1.Lati,loc1.Longi,strLoc];
        [self.webview stringByEvaluatingJavaScriptFromString:str];
    }
    else if ([triggerString isEqualToString:@"ios:createRouteFromLocationsArray"]){
        if(arrRoutePoints.count>0){
            GeoTag *loc1 = (GeoTag *)[arrRoutePoints lastObject];
            NSString *str= [NSString stringWithFormat:@"createRouteToDestLocArray('%@','%@')",loc1.Lati,loc1.Longi];
            [self.webview stringByEvaluatingJavaScriptFromString:str];
        }
    }
    else if ([triggerString isEqualToString:@"ios:NoAddressSelcted"]){
        [[[UIAlertView alloc] initWithTitle:@"Guardian"
                                    message:@"Please select destination location with tap on map"
                                   delegate:nil
                          cancelButtonTitle:@"Ok"
                          otherButtonTitles:nil, nil] show];

    }
    return YES;
}


-(IBAction)routeBtnCicked:(UIButton *)sender{
    if(arrRoutePoints.count>0){
        GeoTag *loc1 = (GeoTag *)[arrRoutePoints lastObject];
        NSString *str= [NSString stringWithFormat:@"createRouteToSelectedAddress('%@','%@')",loc1.Lati,loc1.Longi];
        [self.webview stringByEvaluatingJavaScriptFromString:str];
    }
//    [self.webview stringByEvaluatingJavaScriptFromString:@"createRouteToSelectedAddress()"];
    
//    if(destValueAssigned){
//        for (NSObject *obj in [self.objMapView annotations]) {
//            if([obj isKindOfClass:[AnnoPin class]]){
//                AnnoPin *pin = (AnnoPin *)obj;
//                if(pin.nTag>2){
//                    [self.objMapView removeAnnotation:pin];
//                }
//                
//            }
//        }
//        
//        AnnoPin *pin = [[AnnoPin alloc]initWithCordinatep:coordinateUpto];
//        pin.nTag = 3;
//        pin.title =@"Destination";
//        [self.objMapView addAnnotation:pin];
//        destValueAssigned = NO;
//        isRouteEnabled = YES;
//        [self getDirectionsfrom:loc.coordinate andTo:coordinateUpto];
//        
//    }
//    else{
//        [[[UIAlertView alloc] initWithTitle:@"Guardian"
//                                    message:@"Please select destination location with long press on map"
//                                   delegate:nil
//                          cancelButtonTitle:@"Ok"
//                          otherButtonTitles:nil, nil] show];
//    }
    
}


-(IBAction)btnBackClicked:(id)sender{
    [self.navigationController popViewControllerAnimated:YES];
}

-(void)postLocationArray {
    if([[NSUserDefaults standardUserDefaults] boolForKey:@"PostLocationConsent"]){
        NSMutableArray *arr = [[NSMutableArray alloc] init];
        NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
        if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
            arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
        }
        if(arr.count>0){
            
            NSMutableArray *arrLoc  = [[NSMutableArray alloc] init];
            NSMutableArray *latArr = [[NSMutableArray alloc] init];
            NSMutableArray *longArr = [[NSMutableArray alloc] init];
            NSMutableArray *SpdArr = [[NSMutableArray alloc] init];
            NSMutableArray *AltArr = [[NSMutableArray alloc] init];
            NSMutableArray *TSArr = [[NSMutableArray alloc] init];
            NSMutableArray *MsgTypeArr = [[NSMutableArray alloc] init];
            NSMutableArray *AccArr = [[NSMutableArray alloc] init];
            
            
            
            GeoTag *obj = (GeoTag *)[arr lastObject];
            [latArr addObject:obj.Lati];
            [longArr addObject:obj.Longi];
            [SpdArr addObject:obj.Speed];
            [AltArr addObject:obj.Altitude];
            [AccArr addObject:obj.accuracy];
            [MsgTypeArr addObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"TokenId"]]];
            NSNumberFormatter * f = [[NSNumberFormatter alloc] init];
            [f setNumberStyle:NSNumberFormatterDecimalStyle];
            NSNumber * myNumber = [f numberFromString:obj.timeStamp];
            
            [TSArr addObject:myNumber];
            
//            for (int i=0; i< arr.count; i++) {
//                
//                GeoTag *obj = (GeoTag *)[arr objectAtIndex:i];
//                [latArr addObject:obj.Lati];
//                [longArr addObject:obj.Longi];
//                [SpdArr addObject:obj.Speed];
//                [AltArr addObject:obj.Altitude];
//                [MsgTypeArr addObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"TokenId"]]];
//                NSNumberFormatter * f = [[NSNumberFormatter alloc] init];
//                [f setNumberStyle:NSNumberFormatterDecimalStyle];
//                NSNumber * myNumber = [f numberFromString:obj.timeStamp];
//                
//                [TSArr addObject:myNumber];
//                
//                
//            }
            
            
            NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
            
            [dict setObject:latArr forKey:@"Lat"];
            [dict setObject:longArr forKey:@"Long"];
            [dict setObject:SpdArr forKey:@"Spd"];
            [dict setObject:AltArr forKey:@"Alt"];
            [dict setObject:AccArr forKey:@"Accuracy"];
            [dict setObject:TSArr forKey:@"TS"];
            int count = (int)[arr count];
            [dict setObject:[NSNumber numberWithInt:count] forKey:@"LocCnt"];
            [dict setObject:MsgTypeArr forKey:@"IsSOS"];
            [dict setObject:[NSString stringWithFormat:@",0,"] forKey:@"GroupID"];
            [dict setObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]] forKey:@"PID"];
            //            if(startPushpin)
//            [dict setObject:[NSString stringWithFormat:@"B"] forKey:@"Cmd"];
            
            //            else
            //                [dict setObject:[NSString stringWithFormat:@"E"] forKey:@"Cmd"];
            
            if([[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"] ){
                [dict setObject:[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"] forKey:@"Id"];
            }
            else{
                [dict setObject:[NSString stringWithFormat:@"0"] forKey:@"Id"];
            }
            
            [arrLoc addObject:dict];
            
            [[GlobalClass sharedInstance] postLocations:arrLoc andIndex:0];
        }
    }
}

#pragma mark UIImagePickerDelegate Methods
#pragma mark ----

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingImage:(UIImage *)image editingInfo:(NSDictionary *)editingInfo{
    
    [picker dismissViewControllerAnimated:NO completion:nil];
    int width = image.size.width;
    int height = image.size.height;
    if (width > 1000 || height > 1000)
    {
        width = width / 2;
        height =  height / 2 ;
    }
    
    UIImage *img = [[GlobalClass sharedInstance] imageWithImage:image scaledToSize:CGSizeMake(width, height)];
    NSData* imageData = UIImageJPEGRepresentation(img, 0);
    
    const unsigned char *bytes = [imageData bytes];
    NSUInteger length = [imageData length];
    NSMutableArray *byteArray = [NSMutableArray array];
    for (NSUInteger i = 0; i < length; i++)
    {
        [byteArray addObject:[NSNumber numberWithUnsignedChar:bytes[i]]];
    }
    [[GlobalClass sharedInstance] postLocationWithMediaContent:byteArray];
    
    //    @try {
    //
    //    }
    //    @catch (NSException *exception) {
    //        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
    //        NSLog(@"%@",exception);
    //    }
    //    @finally {
    //            }
    
    
    
}


//- (void)handleSingleTap:(UITapGestureRecognizer*)recognizer{
//
//    CGPoint p = [recognizer locationInView:recognizer.view];
//    
//}

- (void)handleLongPressGestures:(UILongPressGestureRecognizer *)sender
{
    if ([sender isEqual:self.lpgr]) {
        if (sender.state == UIGestureRecognizerStateBegan)
        {
            NSLog(@"%@",sender);
            CGPoint p = [sender locationInView:sender.view];
            CLLocationCoordinate2D coordinate = [self.objMapView convertPoint:p toCoordinateFromView:self.objMapView];
            coordinateUpto = coordinate;
            NSLog(@"latitude is: %f  longitude is: %f" ,coordinate.latitude, coordinate.longitude);
            destValueAssigned = YES;
        }
    }
}



-(void) centerMapForCoordinateArray:(CLLocationCoordinate2D *)routes andCount:(int)count{
	MKCoordinateRegion region;
    
	CLLocationDegrees maxLat = -90;
	CLLocationDegrees maxLon = -180;
	CLLocationDegrees minLat = 90;
	CLLocationDegrees minLon = 180;
    
	for(int idx = 0; idx <count; idx++)
	{
		CLLocationCoordinate2D currentLocation = routes[idx];
		if(currentLocation.latitude > maxLat)
			maxLat = currentLocation.latitude;
		if(currentLocation.latitude < minLat)
			minLat = currentLocation.latitude;
		if(currentLocation.longitude > maxLon)
			maxLon = currentLocation.longitude;
		if(currentLocation.longitude < minLon)
			minLon = currentLocation.longitude;
	}
    
    
	region.center.latitude     = (maxLat + minLat) / 2;
	region.center.longitude    = (maxLon + minLon) / 2;
	region.span.latitudeDelta  = maxLat - minLat;
	region.span.longitudeDelta = maxLon - minLon;
	
	[self.objMapView setRegion:region animated:YES];
}

- (MKPolyline *)polylineWithEncodedString:(NSString *)encodedString {
    const char *bytes = [encodedString UTF8String];
    NSUInteger length = [encodedString lengthOfBytesUsingEncoding:NSUTF8StringEncoding];
    NSUInteger idx = 0;
    
    NSUInteger count = length / 4;
    CLLocationCoordinate2D *coords = calloc(count, sizeof(CLLocationCoordinate2D));
    NSUInteger coordIdx = 0;
    
    float latitude = 0;
    float longitude = 0;
    while (idx < length) {
        char byte = 0;
        int res = 0;
        char shift = 0;
        
        do {
            byte = bytes[idx++] - 63;
            res |= (byte & 0x1F) << shift;
            shift += 5;
        } while (byte >= 0x20);
        
        float deltaLat = ((res & 1) ? ~(res >> 1) : (res >> 1));
        latitude += deltaLat;
        
        shift = 0;
        res = 0;
        
        do {
            byte = bytes[idx++] - 0x3F;
            res |= (byte & 0x1F) << shift;
            shift += 5;
        } while (byte >= 0x20);
        
        float deltaLon = ((res & 1) ? ~(res >> 1) : (res >> 1));
        longitude += deltaLon;
        
        float finalLat = latitude * 1E-5;
        float finalLon = longitude * 1E-5;
        
        CLLocationCoordinate2D coord = CLLocationCoordinate2DMake(finalLat, finalLon);
        coords[coordIdx++] = coord;
        
        if (coordIdx == count) {
            NSUInteger newCount = count + 10;
            coords = realloc(coords, newCount * sizeof(CLLocationCoordinate2D));
            count = newCount;
        }
    }
    
    MKPolyline *polyline1 = [MKPolyline polylineWithCoordinates:coords count:coordIdx];
    free(coords);
    
    return polyline1;
}

//- (IBAction)routeButtonPressed:(UIBarButtonItem *)sender {
//    MKDirectionsRequest *directionsRequest = [[MKDirectionsRequest alloc] init];
//    MKPlacemark *placemark = [[MKPlacemark alloc] initWithPlacemark:thePlacemark];
//    [directionsRequest setSource:[MKMapItem mapItemForCurrentLocation]];
//    
//    [directionsRequest setDestination:[[MKMapItem alloc] initWithPlacemark:placemark]];
//    [directionsRequest setDestination:[[MKMapItem alloc] initWithPlacemark:placemark]];
//    directionsRequest.transportType = MKDirectionsTransportTypeAutomobile;
//    MKDirections *directions = [[MKDirections alloc] initWithRequest:directionsRequest];
//    [directions calculateDirectionsWithCompletionHandler:^(MKDirectionsResponse *response, NSError *error) {
//        if (error) {
//            NSLog(@"Error %@", error.description);
//        } else {
//            routeDetails = response.routes.lastObject;
//            [self.objMapView addOverlay:routeDetails.polyline];
//            self.allSteps = @"";
//            for (int i = 0; i < routeDetails.steps.count; i++) {
//                MKRouteStep *step = [routeDetails.steps objectAtIndex:i];
//                NSString *newStep = step.instructions;
//                self.allSteps = [self.allSteps stringByAppendingString:newStep];
//                self.allSteps = [self.allSteps stringByAppendingString:@"\n\n"];
//                NSLog(@"%@",self.allSteps);
//            }
//        }
//    }];
//}


#pragma mark - MapKit

//- (MKAnnotationView *)mapView:(MKMapView *)mapView viewForAnnotation:(id <MKAnnotation>)annotation {
//    MKPinAnnotationView *annView = [[MKPinAnnotationView alloc] initWithAnnotation:annotation reuseIdentifier:@"currentloc"];
//    annView.canShowCallout = YES;
//    annView.animatesDrop = YES;
//    return annView;
//}

//- (MKOverlayView *)mapView:(MKMapView *)mapView
//            viewForOverlay:(id<MKOverlay>)overlay {
//    
//    for (id<MKOverlay> overlayToRemove in self.objMapView.overlays)
//    {
//        if ([overlayToRemove isKindOfClass:[MKPolyline class]])
//        {
//            NSLog(@"%@",overlayToRemove);
//            [self.objMapView removeOverlay:overlayToRemove];
//        }
//    }
//    
//    MKPolylineRenderer *over = [[MKPolylineRenderer alloc] initWithOverlay:overlay];
//    
//    over.lineWidth = 10;
//    if(!isRouteEnabled){
//        over.strokeColor = [UIColor greenColor];
//        over.fillColor = [[UIColor greenColor] colorWithAlphaComponent:0.8f];
//    }
//    else{
//        over.strokeColor = [UIColor redColor];
//        over.fillColor = [[UIColor redColor] colorWithAlphaComponent:0.8f];
//    }
//    isRouteEnabled = NO;
//    
//    return over.;
//}

- (MKOverlayRenderer *)mapView:(MKMapView *)mapView rendererForOverlay:(id<MKOverlay>)overlay
{
    
    
    
    MKPolylineRenderer *over = [[MKPolylineRenderer alloc] initWithOverlay:overlay];
    
    over.lineWidth = 5;
    if(!isRouteEnabled){
        over.strokeColor = [UIColor greenColor];
        over.fillColor = [[UIColor greenColor] colorWithAlphaComponent:0.8f];
    }
    else{
        over.strokeColor = [UIColor redColor];
        over.fillColor = [[UIColor redColor] colorWithAlphaComponent:0.8f];
    }
    isRouteEnabled = NO;
    return over;
}

-(MKAnnotationView *)mapView:(MKMapView *)mV viewForAnnotation:(id <MKAnnotation>)annotation
{
    AnnoPin *objPin = (AnnoPin *)annotation;
    
//    if(objLastAnnote){
//        if(objLastAnnote.nTag>2)
//            if(objPin!=objLastAnnote)
//                [self.objMapView removeAnnotation:objLastAnnote];
//    }
    
    objLastAnnote = objPin;
    
    MKAnnotationView *pinView = nil;
    if(annotation != self.objMapView.userLocation)
    {
        pinView = [[MKAnnotationView alloc] initWithAnnotation:annotation reuseIdentifier:@"currentloc"];
        pinView.canShowCallout = YES;
        if(objPin.nTag ==1)
            pinView.image = [UIImage imageNamed:@"trackpin.png"];
        else if (objPin.nTag==2)
            pinView.image = [UIImage imageNamed:@"trackpinend.png"];
        else if (objPin.nTag==3)
            pinView.image = [UIImage imageNamed:@"destpin.png"];
    }
    else {
    }
    return pinView;
}

//#pragma mark Calculate directions
//
//- (void)getDirectionsfrom:(CLLocationCoordinate2D)from andTo:(CLLocationCoordinate2D)to {
//    @try {
//        if([[GlobalClass sharedInstance] connected]){
//            for (id<MKOverlay> overlayToRemove in self.objMapView.overlays)
//            {
//                if ([overlayToRemove isKindOfClass:[MKPolyline class]])
//                {
//                    NSLog(@"%@",overlayToRemove);
//                    MKPolyline *pol = (MKPolyline *)overlayToRemove;
//                    if(![pol.title isEqualToString:@"MainTrack"]){
//                        [self.objMapView removeOverlay:overlayToRemove];
//                        
//                        MKPolyline *polyl = (MKPolyline *)overlayToRemove;
//                        polyl = nil;
//                    }
//                    
//                }
//            }
//            
//            
//            CLLocationCoordinate2D endCoordinate;
//            
//            NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"https://maps.googleapis.com/maps/api/directions/json?origin=%f,%f&destination=%f,%f&sensor=false&mode=driving", from.latitude, from.longitude,to.latitude,to.longitude]];
//            NSURLRequest *request = [NSURLRequest requestWithURL:url];
//            NSURLResponse *response = nil;
//            NSError *error = nil;
//            NSData *responseData =  [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&error];
//            if (!error && responseData) {
//                NSDictionary *responseDict = [NSJSONSerialization JSONObjectWithData:responseData options:NSJSONReadingAllowFragments error:&error];
//                if ([[responseDict valueForKey:@"status"] isEqualToString:@"ZERO_RESULTS"]) {
//                    [[[UIAlertView alloc] initWithTitle:@"Error"
//                                                message:@"Could not route path from your current location"
//                                               delegate:nil
//                                      cancelButtonTitle:@"Close"
//                                      otherButtonTitles:nil, nil] show];
//                    return;
//                }
//                NSInteger points_count = 0;
//                if ([[responseDict objectForKey:@"routes"] count])
//                    points_count = [[[[[[responseDict objectForKey:@"routes"] objectAtIndex:0] objectForKey:@"legs"] objectAtIndex:0] objectForKey:@"steps"] count];
//                
//                
//                if (!points_count) {
//                    [[[UIAlertView alloc] initWithTitle:@"Error"
//                                                message:@"Could not route path from your current location"
//                                               delegate:nil
//                                      cancelButtonTitle:@"Close"
//                                      otherButtonTitles:nil, nil] show];
//                    return;
//                }
//                CLLocationCoordinate2D points[points_count];
//                NSLog(@"routes %@", [[[[responseDict objectForKey:@"routes"] objectAtIndex:0]objectForKey:@"overview_polyline"] objectForKey:@"points"]
//                      );
//                MKPolyline *polyline2 = [self polylineWithEncodedString:[[[[responseDict objectForKey:@"routes"] objectAtIndex:0]objectForKey:@"overview_polyline"] objectForKey:@"points"]];
//                if(isTrackingRoute){
//                    polyline2.title = @"MainTrack";
//                    isTrackingRoute = NO;
//                }
//                else{
//                    polyline2.title = @"Route";
//                }
//                [self.objMapView addOverlay:polyline2];
//                
//                int j = 0;
//                NSArray *steps = nil;
//                if (points_count && [[[[responseDict objectForKey:@"routes"] objectAtIndex:0] objectForKey:@"legs"] count])
//                    steps = [[[[[responseDict objectForKey:@"routes"] objectAtIndex:0] objectForKey:@"legs"] objectAtIndex:0] objectForKey:@"steps"];
//                for (int i = 0; i < points_count; i++) {
//                    
//                    double st_lat = [[[[steps objectAtIndex:i] objectForKey:@"start_location"] valueForKey:@"lat"] doubleValue];
//                    double st_lon = [[[[steps objectAtIndex:i] objectForKey:@"start_location"] valueForKey:@"lng"] doubleValue];
//                    //NSLog(@"lat lon: %f %f", st_lat, st_lon);
//                    if (st_lat > 0.0f && st_lon > 0.0f) {
//                        points[j] = CLLocationCoordinate2DMake(st_lat, st_lon);
//                        j++;
//                    }
//                    double end_lat = [[[[steps objectAtIndex:i] objectForKey:@"end_location"] valueForKey:@"lat"] doubleValue];
//                    double end_lon = [[[[steps objectAtIndex:i] objectForKey:@"end_location"] valueForKey:@"lng"] doubleValue];
//                    
//                    //NSLog(@"lat %f lng %f",end_lat,end_lon);
//                    //if (end_lat > 0.0f && end_lon > 0.0f) {
//                    points[j] = CLLocationCoordinate2DMake(end_lat, end_lon);
//                    endCoordinate = CLLocationCoordinate2DMake(end_lat, end_lon);
//                    j++;
//                    //}
//                }
//                NSLog(@"points Count %d",points_count);
//                //        MKPolyline *polyline = [MKPolyline polylineWithCoordinates:points count:points_count];
//                //        [self.mapView addOverlay:polyline];
//                [self centerMapForCoordinateArray:points andCount:points_count];
//            }
//        }
//        else{
//            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//            [alert show];
//        }
//        
//        
//    }
//    @catch (NSException *exception) {
//        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
//        NSLog(@"%@",exception);
//    }
//    @finally {
//        
//    }
//    
//    //    MKPointAnnotation *endannotation = [[MKPointAnnotation alloc] init];
//    //    endannotation.coordinate = CLLocationCoordinate2DMake(coordinateUpto.latitude, coordinateUpto.longitude);
//    //    endannotation.title = @"End";
//    //    [self.objMapView addAnnotation:endannotation];
//    
//    
//}


#pragma mark Search Functionalities

- (void)searchBarCancelButtonClicked:(UISearchBar *)searchBar
{
    self.searchBar.text = @"";
    [self.searchBar resignFirstResponder];
    [self.searchView removeFromSuperview];
}
- (void) searchBarSearchButtonClicked:(UISearchBar *)theSearchBar
{
    [self.searchBar resignFirstResponder];
    @try {
        if(self.searchBar.text.length>0){
            [self searchServiceFromLocationAndIndex:0];
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }

    
    
    //    [self.tbleViewBuddies reloadData];
}

-(void)searchServiceFromLocationAndIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        if([[GlobalClass sharedInstance] connected]){
            
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            
            NSString *urlString = [NSString stringWithFormat:@"https://dev.virtualearth.net/services/v1/SearchService/SearchService.asmx/Search2?count=15&startingIndex=0&mapBounds=&locationcoordinates=\"%@\"&entityType=\"Business\"&sortorder=&query=&location=\"\"&keyword=\"%@\"&jsonso=r229&jsonp=microsoftMapsNetworkCallback&culture=\"en-us\"&token=AoBFMSS4EOyLV9jxIidneive6OtB21mVyzr520OsUwO51tFxCe9vgShVsHs2rz7U",[NSString stringWithFormat:@"%@,%@",loc.Lati,loc.Longi],[NSString stringWithFormat:@"%@",self.searchBar.text]];
            
            
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:[urlString stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
            [request1 setHTTPMethod:@"GET"];
            [NSURLConnection sendAsynchronousRequest:request1
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                       if(!error && data){
                                           
                                           NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                           
                                           NSLog(@"%@",jsonString);
                                           jsonString = [jsonString stringByReplacingOccurrencesOfString:@"microsoftMapsNetworkCallback("
                                                                                              withString:@""];
                                           jsonString = [jsonString stringByReplacingOccurrencesOfString:@".d},'r229');"
                                                                                              withString:@"}"];
                                           jsonString = [jsonString stringByReplacingOccurrencesOfString:@"E+"
                                                                                              withString:@""];
                                           
                                           
                                           NSError *error;
                                           NSData *da = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
                                           id json = [NSJSONSerialization JSONObjectWithData:da options:NSJSONReadingAllowFragments error:&error];
                                           searchPoints = [[[[json objectForKey:@"response"] objectForKey:@"d"]objectForKey:@"SearchResults"] mutableCopy];
                                       }
                                       else{
                                           [self searchServiceFromLocationAndIndex:(NoOfTimes+1)];
                                       }
                                       dispatch_async(dispatch_get_main_queue(), ^{
                                           // Update the UI
                                           [KVNProgress dismiss];
                                           [self.view addSubview:self.searchView];
                                           [self.tbleView reloadData];
                                       });
                                   }];
        }
        else{
//            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//            [alert show];
        }
    }

}


-(void)drawRoute:(GeoTag *)loc1 andTo:(CLLocationCoordinate2D)cood{
    NSString *str= [NSString stringWithFormat:@"createRouteToDestLocation('%@','%@','%@','%@')",loc1.Lati,loc1.Longi,[NSString stringWithFormat:@"%f",cood.latitude],[NSString stringWithFormat:@"%f",cood.longitude]];
    [self.webview stringByEvaluatingJavaScriptFromString:str];
}


- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    return [searchPoints count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    static NSString *simpleTableIdentifier = @"SimpleTableItem";
    
    SearchLocationCustomCell *cell=(SearchLocationCustomCell *)[tableView dequeueReusableCellWithIdentifier:simpleTableIdentifier];
    if (cell==nil) {
        NSArray *array=[[NSBundle mainBundle]loadNibNamed:@"SearchLocationCustomCell" owner:self options:nil];
        cell=(SearchLocationCustomCell *)[array objectAtIndex:0];
    }
    cell.selectionStyle = UITableViewCellSelectionStyleNone;
    
    
    NSString *addStr = [NSString stringWithFormat:@"%@,%@,%@,%@,%@",[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"Name"],[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"Address"],[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"City"],[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"State"],[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"Country"]];
    
    
    float labelHeight1 = [self calculateHeight:[UIFont fontWithName:@"SegoeUI" size:12.0] andWidth:280 andText:addStr];
    
    [cell.lblName setFrame:CGRectMake(10, 5, 280,labelHeight1)];
    cell.lblName.font = [UIFont fontWithName:@"SegoeUI" size:12.0];
    cell.lblName.numberOfLines = 0;
    cell.lblName.lineBreakMode = NSLineBreakByWordWrapping;
    cell.lblName.text = addStr;
    
    return cell;
}


- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    NSString *addStr = [NSString stringWithFormat:@"%@,%@,%@,%@,%@",[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"Name"],[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"Address"],[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"City"],[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"State"],[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"Country"]];
    
    
    float Height = 5;
    
    float labelHeight1 = [self calculateHeight:[UIFont fontWithName:@"SegoeUI" size:12.0] andWidth:280 andText:addStr];
    Height = Height + labelHeight1 + 5;
    return Height;
}

-(float)calculateHeight:(UIFont *)font andWidth:(float )myWidth andText:(NSString *)str{
    NSAttributedString *attributedText = [[NSAttributedString alloc] initWithString:str attributes:@{
                                                                                                     NSFontAttributeName:font
                                                                                                     }];
    CGRect rect = [attributedText boundingRectWithSize:(CGSize){myWidth, CGFLOAT_MAX}
                                               options:NSStringDrawingUsesLineFragmentOrigin
                                               context:nil];
    return rect.size.height;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    
    CLLocationCoordinate2D coordinate = CLLocationCoordinate2DMake([[[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"Location"] objectForKey:@"Latitude"] floatValue], [[[[searchPoints objectAtIndex:indexPath.row] objectForKey:@"Location"] objectForKey:@"Longitude"] floatValue]);;
    coordinateUpto = coordinate;
    
    
    if(arrRoutePoints.count>0){
        GeoTag *loc1 = (GeoTag *)[arrRoutePoints lastObject];
        [self drawRoute:loc1 andTo:coordinateUpto];
//        [self performSelector:@selector(drawRoute:) withObject:loc1 afterDelay:1];
    }
    
    NSLog(@"latitude is: %f  longitude is: %f" ,coordinate.latitude, coordinate.longitude);
    for (NSObject *obj in [self.objMapView annotations]) {
        if([obj isKindOfClass:[AnnoPin class]]){
            AnnoPin *pin = (AnnoPin *)obj;
            if(pin.nTag>2){
                [self.objMapView removeAnnotation:pin];
            }
            
        }
    }

    
    AnnoPin *pin = [[AnnoPin alloc]initWithCordinatep:coordinateUpto];
    pin.nTag = 3;
    pin.title =@"Destination";
    [self.objMapView addAnnotation:pin];
    destValueAssigned = NO;
    isRouteEnabled = YES;
//    [self getDirectionsfrom:loc.coordinate andTo:coordinateUpto];
    [self.searchView removeFromSuperview];
}

#pragma mark MessageCompose Delegate methods
#pragma mark ---

-(void)showSMS:(NSString*)message ForRecipents:(NSArray *)recipents{
    @try {
        NSArray *arr = [[DBaseInteraction sharedInstance] getAllowancesForProfiles:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
        NSLog(@"%@",arr);
        if(arr.count>0){
            if([[[arr objectAtIndex:0] valueForKey:@"CanSMS"] boolValue]){
                
                if(![MFMessageComposeViewController canSendText]) {
                    UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Your device doesn't support SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                    [warningAlert show];
                    return;
                }
                
                MFMessageComposeViewController *messageController = [[MFMessageComposeViewController alloc] init];
                messageController.messageComposeDelegate = self;
                [messageController setRecipients:recipents];
                [messageController setBody:message];
                
                // Present message view controller on screen
                [self presentViewController:messageController animated:YES completion:nil];
                
            }
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }

}

- (void)messageComposeViewController:(MFMessageComposeViewController *)controller didFinishWithResult:(MessageComposeResult) result
{
    switch (result) {
        case MessageComposeResultCancelled:
            break;
            
        case MessageComposeResultFailed:
        {
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Failed to send SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
            break;
        }
            
        case MessageComposeResultSent:
            break;
            
        default:
            break;
    }
    
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
