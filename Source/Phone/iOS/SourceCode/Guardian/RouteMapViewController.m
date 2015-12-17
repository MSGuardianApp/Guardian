//
//  RouteMapViewController.m
//  Guardian
//
//  Created by PTG on 02/02/15.
//  Copyright (c) 2015 People Tech Group. All rights reserved.
//

#import "RouteMapViewController.h"

@interface RouteMapViewController ()
//@property (nonatomic , weak) IBOutlet MKMapView *objMapView;
@property (nonatomic,retain) IBOutlet UIWebView *webview;
@property (nonatomic , retain) IBOutlet UILabel *lblSpeed;
@property (nonatomic , retain) IBOutlet UILabel *lblAccuracy;

-(IBAction)btnBackClicked:(id)sender;
-(IBAction)btnGPSClicked:(id)sender;
-(IBAction)btnRouteClicked:(id)sender;
@end

@implementation RouteMapViewController
@synthesize polyline = _polyline;
@synthesize webview = _webview;
@synthesize buddyData = _buddyData;
@synthesize IsBuddySOS = _IsBuddySOS;

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
    self.buddyData = [[NSMutableDictionary alloc] init];
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arrRoutePoints = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    isGPSTapped = NO;
    timeStamper = [NSString stringWithFormat:@"0"];
    [[NSNotificationCenter defaultCenter] removeObserver:self];
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(userLocationChanged:)
                                                 name:@"UserLocationUpdate"
                                               object:nil];
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(updateBuddyLocation:)
                                                 name:@"BuddyLocationUpdate"
                                               object:nil];
    
    buddyLocateTimer = [NSTimer timerWithTimeInterval:30.0
                                                      target:self
                                                    selector:@selector(getUserLocationArrayWithTimer:)
                                                    userInfo:nil repeats:YES];
    [[NSRunLoop mainRunLoop] addTimer:buddyLocateTimer forMode:NSRunLoopCommonModes];
    //                                                               [[NSNotificationCenter defaultCenter] postNotificationName:@"BuddyLocationUpdate" object:arrList];

    
    // Do any additional setup after loading the view from its nib.
}

-(void) viewWillAppear:(BOOL)animated
{
	[super viewWillAppear:animated];
    
    self.lblSpeed.font = [UIFont fontWithName:@"SegoeUI" size:self.lblSpeed.font.pointSize];
    self.lblAccuracy.font = [UIFont fontWithName:@"SegoeUI" size:self.lblAccuracy.font.pointSize];
    [self.webview loadRequest:[NSURLRequest requestWithURL:[NSURL fileURLWithPath:[[NSBundle mainBundle]pathForResource:@"map" ofType:@"html"]isDirectory:NO]]];
    
//    if(arrRoutePoints.count>0){
//        
//        loc = [arrRoutePoints lastObject];
//        
//        AnnoPin *pin = [[AnnoPin alloc]initWithCordinatep:CLLocationCoordinate2DMake(loc.coordinate.latitude,loc.coordinate.longitude)];
//        pin.nTag = 1;
//        pin.title =@"IN";
//        [self.objMapView addAnnotation:pin];
//        
//        //Get your location and create a CLLocation
//        MKCoordinateRegion region; //create a region.  No this is not a pointer
//        region.center = loc.coordinate;  // set the region center to your current location
//        MKCoordinateSpan span; // create a range of your view
//        span.latitudeDelta = 37.7749300*5/2 ;  // span dimensions.  I have BASE_RADIUS defined as 0.0144927536 which is equivalent to 1 mile
//        span.longitudeDelta =-122.4194200*5/2;  // span dimensions
//        region.span = span; // Set the region's span to the new span.
//        
//        span = MKCoordinateSpanMake(0, 360/pow(2, 15)*self.objMapView.frame.size.width/256);
//        [self.objMapView setRegion:MKCoordinateRegionMake(region.center, span) animated:YES];
//        
//        
//        
//            
//        
//            
//            AnnoPin *pin2 = [[AnnoPin alloc]initWithCordinatep:CLLocationCoordinate2DMake(self.locA.coordinate.latitude,self.locA.coordinate.longitude)];
//            pin2.nTag = 2;
//            pin2.title =@"END";
//            [self.objMapView addAnnotation:pin2];
//            
//            // remove polyline if one exists
//            [self.objMapView removeOverlay:self.polyline];
//            
////            // create an array of coordinates from allPins
////            CLLocationCoordinate2D coordinates[arrRoutePoints.count];
////            int i = 0;
////            for (CLLocation *locations in arrRoutePoints) {
////                coordinates[i] = locations.coordinate;
////                i++;
////            }
////            isTrackingRoute = YES;
//            // create a polyline with all cooridnates
//            //            MKPolyline *polylin = [MKPolyline polylineWithCoordinates:coordinates count:arrRoutePoints.count];
//            //            [self.objMapView addOverlay:polylin level:MKOverlayLevelAboveRoads];
//            //            self.polyline = polylin;
//            
//            [self getDirectionsfrom:loc.coordinate andTo:self.locA.coordinate];
//    }
}
-(void)viewWillDisappear:(BOOL)animated{
    [super viewWillDisappear:YES];
    [buddyLocateTimer invalidate];
}

-(void)getUserLocationArrayWithTimer:(NSTimer *)timer{
    [self getUserLocationArrayAndIndex:0];
}

-(void)getUserLocationArrayAndIndex:(NSInteger)NoOfTimes
{
    if(NoOfTimes<3){
        @try {
            if([[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
                if([[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
                    if([[GlobalClass sharedInstance] connected]){
                        NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:[NSString stringWithFormat:@"%@/%@/%@",kGetUserLocationArray,self.buddyprofileId,timeStamper]] cachePolicy: NSURLRequestUseProtocolCachePolicy
                                                                            timeoutInterval: 60.0];
                        
                        //                        [[GlobalClass sharedInstance] dateToTicks:[NSDate date]]
                        [request1 setHTTPMethod:@"GET"];
                        
                        [request1 setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
                        [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
                        
                        [NSURLConnection sendAsynchronousRequest:request1
                                                           queue:[[NSOperationQueue alloc] init]
                                               completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                                   if(data){
                                                       id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                                       NSLog(@"%@",object);
                                                       if([timeStamper isEqualToString:[NSString stringWithFormat:@"0"]]){
                                                           [self.buddyData mutableCopy];
                                                           [self.buddyData removeAllObjects];
                                                           self.buddyData = (NSMutableDictionary *)object;
                                                           
                                                           
                                                           if([[self.buddyData objectForKey:@"LocCnt"] integerValue]>0){
                                                               
                                                               
                                                               NSString *strLoc = @"";
                                                               for(int i=0;i<[[self.buddyData objectForKey:@"LocCnt"] integerValue];i++){
                                                                   //            GeoTag *loca = (GeoTag *)[arrRoutePoints objectAtIndex:i];
                                                                   
                                                                   strLoc = [strLoc stringByAppendingString:[NSString stringWithFormat:@"%@-%@-%@",[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"Lat"] objectAtIndex:i]],[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"Long"] objectAtIndex:i]],[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"IsSOS"] objectAtIndex:i]]]];
                                                                   if(i<([[self.buddyData objectForKey:@"LocCnt"] integerValue]-1)){
                                                                       strLoc = [strLoc stringByAppendingString:@","];
                                                                   }
                                                               }
                                                               
                                                               NSString *str= [NSString stringWithFormat:@"createRouteToDestLocArray('%@','%@','%@','Locate')",[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"Lat"] objectAtIndex:0]],[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"Long"] objectAtIndex:0]],strLoc];
                                                            dispatch_async(dispatch_get_main_queue(), ^{
                                                                self.lblAccuracy.text = [NSString stringWithFormat:@"Accuracy : %@",[[[object objectForKey:@"Accuracy"] lastObject] stringValue]];
                                                                self.lblSpeed.text = [NSString stringWithFormat:@"Speed : %@",[[[object objectForKey:@"Spd"] lastObject] stringValue]];
                                                               [self.webview stringByEvaluatingJavaScriptFromString:str];
                                                            });
                                                           }
                                                           timeStamper = [[[object objectForKey:@"TS"] lastObject] stringValue];
                                                       }
                                                       else{
                                                           if([[self.buddyData objectForKey:@"LocCnt"] integerValue]>0){
                                                               
                                                               
                                                               self.lblAccuracy.text = [NSString stringWithFormat:@"%@",[[[object objectForKey:@"Accuracy"] lastObject] stringValue]];
                                                               self.lblSpeed.text = [NSString stringWithFormat:@"%@",[[[object objectForKey:@"Spd"] lastObject] stringValue]];
                                                               
                                                               if([[self.buddyData objectForKey:@"LocCnt"] integerValue]>0){
                                                                   NSString *strLoc = @"";
                                                                   for(int i=0;i<[[self.buddyData objectForKey:@"LocCnt"] integerValue];i++){
                                                                       //            GeoTag *loca = (GeoTag *)[arrRoutePoints objectAtIndex:i];
                                                                       
                                                                       strLoc = [strLoc stringByAppendingString:[NSString stringWithFormat:@"%@-%@-%@",[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"Lat"] objectAtIndex:i]],[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"Long"] objectAtIndex:i]],[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"IsSOS"] objectAtIndex:i]]]];
                                                                       if(i<([[self.buddyData objectForKey:@"LocCnt"] integerValue]-1)){
                                                                           strLoc = [strLoc stringByAppendingString:@","];
                                                                       }
                                                                   }
                                                                   
                                                                   NSString *str= [NSString stringWithFormat:@"drawPolyLineForLocBuddy('%@','%@','%@')",[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"Lat"] objectAtIndex:0]],[NSString stringWithFormat:@"%@",[[self.buddyData objectForKey:@"Long"] objectAtIndex:0]],strLoc];
                                                                   dispatch_async(dispatch_get_main_queue(), ^{
                                                                       self.lblAccuracy.text = [NSString stringWithFormat:@"Accuracy : %@",[[[object objectForKey:@"Accuracy"] lastObject] stringValue]];
                                                                       self.lblSpeed.text = [NSString stringWithFormat:@"Speed : %@",[[[object objectForKey:@"Spd"] lastObject] stringValue]];
                                                                       [self.webview stringByEvaluatingJavaScriptFromString:str];
                                                                   });
                                                                   timeStamper = [[[object objectForKey:@"TS"] lastObject] stringValue];
                                                               }
                                                           }

                                                       }
                                                       
                                                   }
                                                else{
                                                     dispatch_async(dispatch_get_main_queue(), ^{
                                                         [self getUserLocationArrayAndIndex:(NoOfTimes+1)];
                                                     });
                                                    }
                                         dispatch_async(dispatch_get_main_queue(), ^{
                                            [KVNProgress dismiss];
                                        });
                         
                         }];
                        
                        
                    }
                    else{
                            //                        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
                            //                        [alert show];
                        }
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
    
}



-(IBAction)btnGPSClicked:(id)sender{
    if(arrRoutePoints.count>0){
        isGPSTapped = YES;
        GeoTag *loc1 = (GeoTag *)[arrRoutePoints lastObject];
        NSString *str= [NSString stringWithFormat:@"createRouteToDestLocation('%@','%@','%@','%@','TRUE')",loc1.Lati,loc1.Longi,[NSString stringWithFormat:@"%lf",self.locA.coordinate.latitude],[NSString stringWithFormat:@"%lf",self.locA.coordinate.longitude]];
        [self.webview stringByEvaluatingJavaScriptFromString:str];
    }
}
-(IBAction)btnRouteClicked:(id)sender{
    if(arrRoutePoints.count>0){
            GeoTag *loc1 = (GeoTag *)[arrRoutePoints lastObject];
            NSString *str= [NSString stringWithFormat:@"createRouteToSelectedAddress('%@','%@')",loc1.Lati,loc1.Longi];
            [self.webview stringByEvaluatingJavaScriptFromString:str];
    }
}

-(void)userLocationChanged:(NSNotification *)notification{
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arrRoutePoints = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    if(arrRoutePoints.count>0){
        if(isGPSTapped){
            GeoTag *loc1 = (GeoTag *)[arrRoutePoints lastObject];
            NSString *str= [NSString stringWithFormat:@"drawPolyLineForUserLocArray('%@','%@','%@')",loc1.Lati,loc1.Longi,@"false"];
            [self.webview stringByEvaluatingJavaScriptFromString:str];
        }
    }
}

-(void)updateBuddyLocation:(NSNotification *)notification{
    NSMutableArray *arr = (NSMutableArray *)notification.object;
    NSArray *filteredarray = [arr filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(MobileNumber == %@)", self.selPhoneNumber]];
    
    if(filteredarray.count>0){
        
        CLLocation *loc1 = [[CLLocation alloc] initWithLatitude:[[[[[filteredarray objectAtIndex:0] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Lat"] doubleValue] longitude:[[[[[filteredarray objectAtIndex:0] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Long"] doubleValue]];
        self.locA = loc1;
        if(isGPSTapped){
            NSString *str= [NSString stringWithFormat:@"drawPolyLine('%@','%@','%@')",[[[[filteredarray objectAtIndex:0] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Lat"],[[[[filteredarray objectAtIndex:0] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Long"],@"true"];
            [self.webview stringByEvaluatingJavaScriptFromString:str];
        }
        
    }
}



- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType {
    
    NSString *triggerString=[[request URL] absoluteString];
    if([triggerString isEqualToString:@"ios:mapLoaded"]) {
            NSString *str= [NSString stringWithFormat:@"locateBuddyOrUser('%@','%@','TRUE')",[NSString stringWithFormat:@"%f",self.locA.coordinate.latitude],[NSString stringWithFormat:@"%f",self.locA.coordinate.longitude]];
                [self.webview stringByEvaluatingJavaScriptFromString:str];
        
                [self getUserLocationArrayAndIndex:0];
    }
    else if ([triggerString isEqualToString:@"ios:NoAddressSelcted"]){
        [[[UIAlertView alloc] initWithTitle:@"Guardian"
                                    message:@"Please select destination location with tap on map"
                                   delegate:nil
                          cancelButtonTitle:@"Ok"
                          otherButtonTitles:nil, nil] show];
        
    }
//    else if ([triggerString isEqualToString:@"ios:createRouteFromLocationsArray"]){
//        if(arrRoutePoints.count>0){
//            GeoTag *obj = (GeoTag *)[arrRoutePoints lastObject];
//            NSString *str= [NSString stringWithFormat:@"createRouteToDestLocArray('%@','%@')",obj.Lati,obj.Longi];
//            [self.webview stringByEvaluatingJavaScriptFromString:str];
//        }
//    }
//    else{
//        if(arrRoutePoints.count>0){
//            GeoTag *obj = (GeoTag *)[arrRoutePoints lastObject];
//            [self performSelector:@selector(drawRoute:) withObject:obj afterDelay:1];
//        }
//    }
    return YES;
    
}
-(void)drawRoute:(GeoTag *)obj{
    NSString *str= [NSString stringWithFormat:@"createRouteToDestLocation('%@','%@','%@','%@')",obj.Lati,obj.Longi,[NSString stringWithFormat:@"%f",self.locA.coordinate.latitude],[NSString stringWithFormat:@"%f",self.locA.coordinate.longitude]];
    [self.webview stringByEvaluatingJavaScriptFromString:str];
}


//- (MKOverlayRenderer *)mapView:(MKMapView *)mapView rendererForOverlay:(id<MKOverlay>)overlay
//{
//    
//    MKPolylineRenderer *over = [[MKPolylineRenderer alloc] initWithOverlay:overlay];
//    
//    over.lineWidth = 5;
////    if(!isRouteEnabled){
////        over.strokeColor = [UIColor greenColor];
////        over.fillColor = [[UIColor greenColor] colorWithAlphaComponent:0.8f];
////    }
////    else{
//        over.strokeColor = [UIColor redColor];
//        over.fillColor = [[UIColor redColor] colorWithAlphaComponent:0.8f];
////    }
////    isRouteEnabled = NO;
//    return over;
//}
//
//-(MKAnnotationView *)mapView:(MKMapView *)mV viewForAnnotation:(id <MKAnnotation>)annotation
//{
//    
//    
//    AnnoPin *objPin = (AnnoPin *)annotation;
//    
////    if(objLastAnnote){
////        if(objLastAnnote.nTag!=1 && objLastAnnote.nTag!=2)
////            if(objPin!=objLastAnnote)
////                [self.objMapView removeAnnotation:objLastAnnote];
////    }
////    
////    objLastAnnote = objPin;
//    
//    MKAnnotationView *pinView = nil;
//    if(annotation != self.objMapView.userLocation)
//    {
//        pinView = [[MKAnnotationView alloc] initWithAnnotation:annotation reuseIdentifier:@"currentloc"];
//        pinView.canShowCallout = YES;
//        if(objPin.nTag ==1)
//            pinView.image = [UIImage imageNamed:@"trackpin.png"];
//        else if (objPin.nTag==2)
//            pinView.image = [UIImage imageNamed:@"trackpinend.png"];
//    }
//    else {
//    }
//    return pinView;
//}
//
//#pragma mark Calculate directions
//
//- (void)getDirectionsfrom:(CLLocationCoordinate2D)from andTo:(CLLocationCoordinate2D)to {
//    
//    
//    for (id<MKOverlay> overlayToRemove in self.objMapView.overlays)
//    {
//        if ([overlayToRemove isKindOfClass:[MKPolyline class]])
//        {
//            NSLog(@"%@",overlayToRemove);
//            MKPolyline *pol = (MKPolyline *)overlayToRemove;
//            if(![pol.title isEqualToString:@"MainTrack"]){
//                [self.objMapView removeOverlay:overlayToRemove];
//                
//                MKPolyline *polyl = (MKPolyline *)overlayToRemove;
//                polyl = nil;
//            }
//            
//        }
//    }
//    
//    
//    CLLocationCoordinate2D endCoordinate;
//    
//    NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"https://maps.googleapis.com/maps/api/directions/json?origin=%f,%f&destination=%f,%f&sensor=false&mode=driving", from.latitude, from.longitude,to.latitude,to.longitude]];
//    NSURLRequest *request = [NSURLRequest requestWithURL:url];
//    NSURLResponse *response = nil;
//    NSError *error = nil;
//    NSData *responseData =  [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&error];
//    if (!error) {
//        NSDictionary *responseDict = [NSJSONSerialization JSONObjectWithData:responseData options:NSJSONReadingAllowFragments error:&error];
//        if ([[responseDict valueForKey:@"status"] isEqualToString:@"ZERO_RESULTS"]) {
//            [[[UIAlertView alloc] initWithTitle:@"Error"
//                                        message:@"Could not route path from your current location"
//                                       delegate:nil
//                              cancelButtonTitle:@"Close"
//                              otherButtonTitles:nil, nil] show];
//            return;
//        }
//        int points_count = 0;
//        if ([[responseDict objectForKey:@"routes"] count])
//            points_count = [[[[[[responseDict objectForKey:@"routes"] objectAtIndex:0] objectForKey:@"legs"] objectAtIndex:0] objectForKey:@"steps"] count];
//        
//        
//        if (!points_count) {
//            [[[UIAlertView alloc] initWithTitle:@"Error"
//                                        message:@"Could not route path from your current location"
//                                       delegate:nil
//                              cancelButtonTitle:@"Close"
//                              otherButtonTitles:nil, nil] show];
//            return;
//        }
//        CLLocationCoordinate2D points[points_count];
//        NSLog(@"routes %@", [[[[responseDict objectForKey:@"routes"] objectAtIndex:0]objectForKey:@"overview_polyline"] objectForKey:@"points"]
//              );
//        MKPolyline *polyline2 = [self polylineWithEncodedString:[[[[responseDict objectForKey:@"routes"] objectAtIndex:0]objectForKey:@"overview_polyline"] objectForKey:@"points"]];
////        if(isTrackingRoute){
////            polyline2.title = @"MainTrack";
////            isTrackingRoute = NO;
////        }
////        else{
////            polyline2.title = @"Route";
////        }
//        [self.objMapView addOverlay:polyline2];
//        
//        int j = 0;
//        NSArray *steps = nil;
//        if (points_count && [[[[responseDict objectForKey:@"routes"] objectAtIndex:0] objectForKey:@"legs"] count])
//            steps = [[[[[responseDict objectForKey:@"routes"] objectAtIndex:0] objectForKey:@"legs"] objectAtIndex:0] objectForKey:@"steps"];
//        for (int i = 0; i < points_count; i++) {
//            
//            double st_lat = [[[[steps objectAtIndex:i] objectForKey:@"start_location"] valueForKey:@"lat"] doubleValue];
//            double st_lon = [[[[steps objectAtIndex:i] objectForKey:@"start_location"] valueForKey:@"lng"] doubleValue];
//            //NSLog(@"lat lon: %f %f", st_lat, st_lon);
//            if (st_lat > 0.0f && st_lon > 0.0f) {
//                points[j] = CLLocationCoordinate2DMake(st_lat, st_lon);
//                j++;
//            }
//            double end_lat = [[[[steps objectAtIndex:i] objectForKey:@"end_location"] valueForKey:@"lat"] doubleValue];
//            double end_lon = [[[[steps objectAtIndex:i] objectForKey:@"end_location"] valueForKey:@"lng"] doubleValue];
//            
//            //NSLog(@"lat %f lng %f",end_lat,end_lon);
//            //if (end_lat > 0.0f && end_lon > 0.0f) {
//            points[j] = CLLocationCoordinate2DMake(end_lat, end_lon);
//            endCoordinate = CLLocationCoordinate2DMake(end_lat, end_lon);
//            j++;
//            //}
//        }
//        NSLog(@"points Count %d",points_count);
//        //        MKPolyline *polyline = [MKPolyline polylineWithCoordinates:points count:points_count];
//        //        [self.mapView addOverlay:polyline];
//        [self centerMapForCoordinateArray:points andCount:points_count];
//    }
//}
//
//- (MKPolyline *)polylineWithEncodedString:(NSString *)encodedString {
//    const char *bytes = [encodedString UTF8String];
//    NSUInteger length = [encodedString lengthOfBytesUsingEncoding:NSUTF8StringEncoding];
//    NSUInteger idx = 0;
//    
//    NSUInteger count = length / 4;
//    CLLocationCoordinate2D *coords = calloc(count, sizeof(CLLocationCoordinate2D));
//    NSUInteger coordIdx = 0;
//    
//    float latitude = 0;
//    float longitude = 0;
//    while (idx < length) {
//        char byte = 0;
//        int res = 0;
//        char shift = 0;
//        
//        do {
//            byte = bytes[idx++] - 63;
//            res |= (byte & 0x1F) << shift;
//            shift += 5;
//        } while (byte >= 0x20);
//        
//        float deltaLat = ((res & 1) ? ~(res >> 1) : (res >> 1));
//        latitude += deltaLat;
//        
//        shift = 0;
//        res = 0;
//        
//        do {
//            byte = bytes[idx++] - 0x3F;
//            res |= (byte & 0x1F) << shift;
//            shift += 5;
//        } while (byte >= 0x20);
//        
//        float deltaLon = ((res & 1) ? ~(res >> 1) : (res >> 1));
//        longitude += deltaLon;
//        
//        float finalLat = latitude * 1E-5;
//        float finalLon = longitude * 1E-5;
//        
//        CLLocationCoordinate2D coord = CLLocationCoordinate2DMake(finalLat, finalLon);
//        coords[coordIdx++] = coord;
//        
//        if (coordIdx == count) {
//            NSUInteger newCount = count + 10;
//            coords = realloc(coords, newCount * sizeof(CLLocationCoordinate2D));
//            count = newCount;
//        }
//    }
//    
//    MKPolyline *polyline1 = [MKPolyline polylineWithCoordinates:coords count:coordIdx];
//    free(coords);
//    
//    return polyline1;
//}
//
//-(void) centerMapForCoordinateArray:(CLLocationCoordinate2D *)routes andCount:(int)count{
//	MKCoordinateRegion region;
//    
//	CLLocationDegrees maxLat = -90;
//	CLLocationDegrees maxLon = -180;
//	CLLocationDegrees minLat = 90;
//	CLLocationDegrees minLon = 180;
//    
//	for(int idx = 0; idx <count; idx++)
//	{
//		CLLocationCoordinate2D currentLocation = routes[idx];
//		if(currentLocation.latitude > maxLat)
//			maxLat = currentLocation.latitude;
//		if(currentLocation.latitude < minLat)
//			minLat = currentLocation.latitude;
//		if(currentLocation.longitude > maxLon)
//			maxLon = currentLocation.longitude;
//		if(currentLocation.longitude < minLon)
//			minLon = currentLocation.longitude;
//	}
//    
//    
//	region.center.latitude     = (maxLat + minLat) / 2;
//	region.center.longitude    = (maxLon + minLon) / 2;
//	region.span.latitudeDelta  = maxLat - minLat;
//	region.span.longitudeDelta = maxLon - minLon;
//	
//	[self.objMapView setRegion:region animated:YES];
//}




-(IBAction)btnBackClicked:(id)sender{
    [self.navigationController popViewControllerAnimated:YES];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
