//
//  LocateViewController.m
//  Guardian
//
//  Created by PTG on 22/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "LocateViewController.h"
#import "LocateCustomCell.h"
#import "RegisterViewController.h"
#import "RouteMapViewController.h"

@interface LocateViewController ()
@property (nonatomic , retain) IBOutlet UITableView *tblLocate;

@property (nonatomic , weak) IBOutlet UIView *viewMicrosoftTile;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect1;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect2;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect3;
@property (nonatomic , retain) IBOutlet UILabel *lblTitle;
@property (nonatomic , retain) IBOutlet UILabel *lblNobuddyTxt;
@property (nonatomic , retain) IBOutlet UIButton *btnPrivacy;

-(IBAction)btnPrivacyPoilcyClicked:(id)sender;
-(IBAction)refreshBtnCicked:(UIButton *)sender;
-(IBAction)homeBtnCicked:(UIButton *)sender;
@end







@implementation LocateViewController

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
    arrList = [[NSMutableArray alloc] init];
    
//    NSString *num =  @"635549232959371774";
//    num = [num substringToIndex:[num length] - 4];
//    
//    NSDate *date = [self displayDate:[num longLongValue]];
//    NSLog(@"%@",date);
////     NSDate *date = [NSDate dateWithTimeIntervalSince1970:[num doubleValue]];
//    
//    NSDateFormatter *forma = [[NSDateFormatter alloc] init];
//    [forma setLocale:[NSLocale currentLocale]];
//    [forma setDateFormat:@"yyyyMMdd HH:mm:ss"];
//    
//    NSString *str  = [forma stringFromDate:date];
//    
//    NSLog(@"%@",str);
    
    
    
    
    self.btnPrivacy.titleLabel.font = [UIFont fontWithName:@"SegoeUI" size:self.btnPrivacy.titleLabel.font.pointSize];
    self.lblTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblTitle.font.pointSize];
    
    // Do any additional setup after loading the view from its nib.
}

-(void)viewWillAppear:(BOOL)animated{
    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
        [self.viewMicrosoftTile setFrame:CGRectMake(0, 80, 320,[UIScreen mainScreen].applicationFrame.size.height-65)];
        [self.view addSubview:self.viewMicrosoftTile];
    }
    else{
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        if([defaults boolForKey:@"ProfileInserted"]){
            [self.viewMicrosoftTile removeFromSuperview];
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Locating Buddies...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            [self getBuddiesToLocateLastLocationAndIndex:0];
//            [NSTimer scheduledTimerWithTimeInterval:30 target: self
//        selector:@selector(getBuddiesToLocateLastLocationAndIndex:)userInfo:0 repeats: YES];
            AppDelegate *appDel = (AppDelegate *)[[UIApplication sharedApplication] delegate];
            
            
            appDel.locateBuddyTimer = [NSTimer timerWithTimeInterval:30.0
                                                     target:self
                                                   selector:@selector(getBuddiesToLocateLastLocationWithTimer:)
                                                   userInfo:nil repeats:YES];
            [[NSRunLoop mainRunLoop] addTimer:appDel.locateBuddyTimer forMode:NSRunLoopCommonModes];
        }
        else{
            [self.viewMicrosoftTile setFrame:CGRectMake(0, 80, 320,[UIScreen mainScreen].applicationFrame.size.height-65)];
            [self.view addSubview:self.viewMicrosoftTile];
        }
    }
}
-(void)viewWillDisappear:(BOOL)animated{
//    AppDelegate *appDel = (AppDelegate *)[[UIApplication sharedApplication] delegate];
//    [appDel.locateBuddyTimer invalidate];
}

- (NSDate *)displayDate:(double)unixMilliseconds {
    
    NSDate *date = [NSDate dateWithTimeIntervalSince1970:unixMilliseconds / 1000.0];
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    
    [dateFormatter setDateStyle:NSDateFormatterShortStyle];
    [dateFormatter setTimeStyle:NSDateFormatterShortStyle];
    [dateFormatter setLocale:[NSLocale currentLocale]];
    
    NSLog(@"The date is %@", [dateFormatter stringFromDate:date]);
    //  NSDate *returnValue = [dateFormatter stringFromDate:date];
    return date;
}
-(void)getBuddiesToLocateLastLocationWithTimer:(NSTimer *)timer{
    [self getBuddiesToLocateLastLocationAndIndex:0];
}

-(void)getBuddiesToLocateLastLocationAndIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        @try {
            if([[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
                if([[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
                    if([[GlobalClass sharedInstance] connected]){
                        
                        NSLog(@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]);
                        NSArray *arr = [[DBaseInteraction sharedInstance] getUserTable:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                        
                        NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:[NSString stringWithFormat:@"%@%@/%@",[NSString stringWithFormat:@"%@",kGetBuddiesToLocateLastLocation],[[arr objectAtIndex:0] objectForKey:@"UserId"],[[GlobalClass sharedInstance] dateToTicks:[NSDate date]]]]
                                                                                cachePolicy: NSURLRequestUseProtocolCachePolicy
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
                                                       NSDictionary *dict2 = [[NSDictionary alloc] init];
                                                       dict2 = (NSDictionary *)object;
                                                       if(dict2.count>0){
                                                           if(![[dict2 objectForKey:@"List"] isKindOfClass:[NSNull class]]){
                                                               if([[dict2 objectForKey:@"List"] count]>0){
                                                                   arrList = [dict2 objectForKey:@"List"];
                                                               }
                                                               else{
                                                                   [arrList removeAllObjects];
                                                               }
                                                           }
                                                           else{
                                                               [arrList removeAllObjects];
                                                           }
                                                           [arrList mutableCopy];
                                                           
                                                           NSMutableArray *arrSorted = [[NSMutableArray alloc] init];
                                                           
                                                           NSMutableArray *arrCopy = [[NSMutableArray alloc]initWithArray:arrList];
                                                           
                                                           
                                                           for (NSMutableDictionary *dict in arrList) {
                                                               if([[dict objectForKey:@"IsSOSOn"] integerValue]==1){
                                                                   [arrSorted addObject:dict];
                                                                   [arrCopy removeObjectIdenticalTo:dict];
                                                               }
                                                           }
                                                           for (NSMutableDictionary *dict3 in arrList) {
                                                               if([[dict3 objectForKey:@"IsTrackingOn"] integerValue]==1){
                                                                   if([[dict3 objectForKey:@"IsSOSOn"] integerValue]!=1){
                                                                       [arrSorted addObject:dict3];
                                                                       [arrCopy removeObjectIdenticalTo:dict3];
                                                                   }
                                                               }
                                                           }
                                                           for (NSMutableDictionary *dict in arrCopy) {
                                                               [arrSorted addObject:dict];
                                                           }
                                                           
                                                           arrList = [arrSorted mutableCopy];
//                                                           if(arrList.count>0){
//                                                               self.lblNobuddyTxt.hidden = YES;
//                                                           }
//                                                           else{
//                                                               self.lblNobuddyTxt.hidden = NO;
//                                                           }
                                                           arrSorted = nil;
                                                           arrCopy = nil;
                                                           
                                                           dispatch_async(dispatch_get_main_queue(), ^{
                                                               [self.tblLocate reloadData];
                                                               
                                                               if(arrList.count>0){
                                                                   self.lblNobuddyTxt.hidden = YES;
                                                               }
                                                               else{
                                                                   self.lblNobuddyTxt.hidden = NO;
                                                               }
                                                               
                                                               
                                                               [KVNProgress dismiss];
//                                                               [[NSNotificationCenter defaultCenter] postNotificationName:@"BuddyLocationUpdate" object:arrList];
                                                           });
                                                       }
                                                       
                                                   }
                                                   else{
                                                       dispatch_async(dispatch_get_main_queue(), ^{
                                                           [self getBuddiesToLocateLastLocationAndIndex:(NoOfTimes+1)];
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


-(IBAction)refreshBtnCicked:(UIButton *)sender{
    [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                      KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                      KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
    [self getBuddiesToLocateLastLocationAndIndex:0];
}

-(IBAction)btnPrivacyPoilcyClicked:(id)sender{
    NSString* launchUrl = @"https://guardianportal.cloudapp.net/privacy.htm";
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString: launchUrl]];
}

- (IBAction)onClickSignInButton:(id)sender
{
    MicrosoftLiveConnect *obj = [[MicrosoftLiveConnect alloc] init];
    [self.navigationController pushViewController:obj animated:NO];
}

-(IBAction)homeBtnCicked:(UIButton *)sender{
    [self.navigationController popViewControllerAnimated:YES];
}



-(UIViewController*) getRootViewController {
    return [UIApplication sharedApplication].keyWindow.rootViewController;
}

#pragma mark UITableViewCells -UITableViewDelegate

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section;
{
    return arrList.count;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath  {
    
    static NSString *cellIdentifier=@"Cell";
    LocateCustomCell *cell=(LocateCustomCell *)[tableView dequeueReusableCellWithIdentifier:cellIdentifier];
    if (cell==nil) {
        NSArray *array=[[NSBundle mainBundle]loadNibNamed:@"LocateCustomCell" owner:self options:nil];
        cell=(LocateCustomCell *)[array objectAtIndex:0];
    }
    
    cell.selectionStyle = UITableViewCellSelectionStyleNone;
    
    cell.lblName.font = [UIFont fontWithName:@"SegoeUI" size:cell.lblName.font.pointSize];
    cell.lblPhoneNumber.font = [UIFont fontWithName:@"SegoeUI" size:cell.lblPhoneNumber.font.pointSize];
    cell.lblAddress.font = [UIFont fontWithName:@"SegoeUI" size:cell.lblAddress.font.pointSize];
    
    cell.lblName.text = [[arrList objectAtIndex:indexPath.row] objectForKey:@"Name"];
    
    cell.lblPhoneNumber.text = [[arrList objectAtIndex:indexPath.row] objectForKey:@"MobileNumber"];
    
    [cell.btnMap addTarget:self action:@selector(accessoryMapButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
    cell.btnMap.tag = indexPath.row;
    
    [cell.btnCall addTarget:self action:@selector(accessoryCallButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
    cell.btnCall.tag = indexPath.row;
    
    [cell.btnSMS addTarget:self action:@selector(accessorySMSButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
    cell.btnSMS.tag = indexPath.row;
    
    [cell.btnMail addTarget:self action:@selector(accessoryMailButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
    cell.btnMail.tag = indexPath.row;
    
    
        
    
    cell.viewBG.layer.borderWidth =1.0f;
//    cell.viewBG.layer.borderColor = [UIColor whiteColor].CGColor;
    
    cell.btnMap.layer.borderWidth =1.0f;
    cell.btnMap.layer.borderColor = [UIColor whiteColor].CGColor;
    
    cell.btnCall.layer.borderWidth =1.0f;
    cell.btnCall.layer.borderColor = [UIColor whiteColor].CGColor;
    
    cell.btnSMS.layer.borderWidth =1.0f;
    cell.btnSMS.layer.borderColor = [UIColor whiteColor].CGColor;
    
    cell.btnMail.layer.borderWidth =1.0f;
    cell.btnMail.layer.borderColor = [UIColor whiteColor].CGColor;
    
    if([[[arrList objectAtIndex:indexPath.row] objectForKey:@"IsSOSOn"] integerValue]==1){
        cell.viewBG.layer.borderColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1].CGColor;
        cell.lblName.textColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
        cell.lblPhoneNumber.textColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    }
    else if([[[arrList objectAtIndex:indexPath.row] objectForKey:@"IsTrackingOn"] integerValue]==1){
        cell.viewBG.layer.borderColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1].CGColor;
        cell.lblName.textColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
        cell.lblPhoneNumber.textColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
    }
    else{
        cell.viewBG.layer.borderColor = [UIColor whiteColor].CGColor;
        cell.lblName.textColor = [UIColor whiteColor];
        cell.lblPhoneNumber.textColor = [UIColor whiteColor];
    }
    
    if([[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] count] > 0 ){
        if(cell.lblAddress.text.length == 0){
            CLLocation *LocationAtual = [[CLLocation alloc] initWithLatitude:[[[[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Lat"] doubleValue] longitude:[[[[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Long"] doubleValue]];
            @try {
                [self reversegeoCoding:LocationAtual andIndex:indexPath andTimeStamp:[[[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"TimeStamp"]];
            }
            @catch (NSException *exception) {
                [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
                NSLog(@"%@",exception);
            }
            @finally {
                
            }
            
        }
    }
    
    
    return cell;
    
}

- (void) accessoryMapButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    @try {
        NSIndexPath * indexPath = [self.tblLocate indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblLocate]];
        if ( indexPath == nil )
            return;
        
        //    CLLocationCoordinate2D coordinate =    CLLocationCoordinate2DMake([[[[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Lat"] doubleValue],[[[[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Long"] doubleValue]);
        if([[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] count]>0){
            CLLocation *loc = [[CLLocation alloc] initWithLatitude:[[[[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Lat"] doubleValue] longitude:[[[[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Long"] doubleValue]];
            RouteMapViewController *objRouteMapViewController = [[RouteMapViewController alloc] initWithNibName:@"RouteMapViewController" bundle:nil];
            objRouteMapViewController.locA = loc;
            objRouteMapViewController.buddyprofileId = [NSString stringWithFormat:@"%ld",[[[arrList objectAtIndex:indexPath.row] objectForKey:@"ProfileID"] longValue]];
            objRouteMapViewController.IsBuddySOS = [NSString stringWithFormat:@"%ld",(long)[[[arrList objectAtIndex:indexPath.row] objectForKey:@"IsSOSOn"] integerValue]];
            objRouteMapViewController.selPhoneNumber = [[arrList objectAtIndex:indexPath.row] objectForKey:@"MobileNumber"];
            [self.navigationController pushViewController:objRouteMapViewController animated:YES];
        }
        

    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
//    //create MKMapItem out of coordinates
//    MKPlacemark* placeMark = [[MKPlacemark alloc] initWithCoordinate:coordinate addressDictionary:nil];
//    MKMapItem* destination =  [[MKMapItem alloc] initWithPlacemark:placeMark];
//    if([destination respondsToSelector:@selector(openInMapsWithLaunchOptions:)])
//    {
//        [destination openInMapsWithLaunchOptions:@{MKLaunchOptionsDirectionsModeKey:MKLaunchOptionsDirectionsModeDriving}];
//        //using iOS6 native maps app
////        if(_mode == 1)
////        {
////            [destination openInMapsWithLaunchOptions:@{MKLaunchOptionsDirectionsModeKey:MKLaunchOptionsDirectionsModeWalking}];
////            
////        }
////        if(_mode == 2)
////        {
////            [destination openInMapsWithLaunchOptions:@{MKLaunchOptionsDirectionsModeKey:MKLaunchOptionsDirectionsModeDriving}];
////            
////        }
////        if(_mode == 3)
////        {
////            [destination openInMapsWithLaunchOptions:@{MKLaunchOptionsDirectionsModeKey:MKLaunchOptionsDirectionsModeDriving}];
////            
////        }
//        
//    } else{
//        
//        //using iOS 5 which has the Google Maps application
//        NSString* url = [NSString stringWithFormat: @"http://maps.google.com/maps?saddr=Current+Location&daddr=%f,%f", [[[[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Lat"] doubleValue], [[[[[arrList objectAtIndex:indexPath.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"Long"] doubleValue]];
//        [[UIApplication sharedApplication] openURL: [NSURL URLWithString: url]];
//    }
}

- (void) accessoryCallButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    NSIndexPath * indexPath = [self.tblLocate indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblLocate]];
    if ( indexPath == nil )
        return;
   
    UIDevice *device = [UIDevice currentDevice];
    
    NSString *cellNameStr = [NSString stringWithFormat:@"%@",[[arrList objectAtIndex:indexPath.row] objectForKey:@"MobileNumber"]];
    
    if ([[device model] isEqualToString:@"iPhone"] ) {
        
        NSString *phoneNumber = [@"telprompt://" stringByAppendingString:cellNameStr];
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:phoneNumber]];
        
    } else {
        
        UIAlertView *warning =[[UIAlertView alloc] initWithTitle:@"Call" message:@"Your device doesn't support this feature." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
        
        [warning show];
    }
    
}

- (void) accessorySMSButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    @try {
        NSIndexPath * indexPath = [self.tblLocate indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblLocate]];
        if ( indexPath == nil )
            return;
        
        if(![MFMessageComposeViewController canSendText]) {
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Your device doesn't support SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
            return;
        }
        
        MFMessageComposeViewController *messageController = [[MFMessageComposeViewController alloc] init];
        messageController.messageComposeDelegate = self;
        [messageController setRecipients:[NSArray arrayWithObject:[[arrList objectAtIndex:indexPath.row] objectForKey:@"MobileNumber"]]];
        [messageController setBody:@"I'm reaching to help you."];
        
        // Present message view controller on screen
        [self presentViewController:messageController animated:YES completion:nil];
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
    
}

- (void) accessoryMailButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    @try {
        NSIndexPath * indexPath = [self.tblLocate indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblLocate]];
        if ( indexPath == nil )
            return;
        
        if([[arrList objectAtIndex:indexPath.row] objectForKey:@"Email"] && [[[arrList objectAtIndex:indexPath.row] objectForKey:@"Email"] length]>0){
            NSString *emailTitle = @"Guardian";
            // Email Content
            NSString *messageBody = @"I'm reaching to help you.";
            // To address
            NSArray *toRecipents = [NSArray arrayWithObject:[[arrList objectAtIndex:indexPath.row] objectForKey:@"Email"]];
            if ([MFMailComposeViewController canSendMail])
            {
                MFMailComposeViewController *mc = [[MFMailComposeViewController alloc] init];
                //    [mc.navigationController.navigationBar setBackgroundColor:[UIColor colorWithRed:255.0f/255.0f green:130.0f/255.0f blue:0.0f/255.0f alpha:1]];
                //    [mc.navigationBar setTintColor:[UIColor colorWithRed:255.0f/255.0f green:130.0f/255.0f blue:0.0f/255.0f alpha:1]];
                
                mc.mailComposeDelegate = self;
                [mc setSubject:emailTitle];
                [mc setMessageBody:messageBody isHTML:NO];
                [mc setToRecipients:toRecipents];
                
                // Present mail view controller on screen
                [self presentViewController:mc animated:YES completion:NULL];
            }
			else{
                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Mail not integrated in your device" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil];
                [alert show];
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

- (void) reversegeoCoding:(CLLocation *)loc andIndex:(NSIndexPath *)index andTimeStamp:(NSString *)timestamp{
    CLGeocoder *geocoder = [[CLGeocoder alloc] init];
    
    //Block address

    
    [geocoder reverseGeocodeLocation:loc completionHandler:
     ^(NSArray *placemarks, NSError *error) {
         
         //Get address
         CLPlacemark *placemark = [placemarks objectAtIndex:0];
         
         NSLog(@"Placemark array: %@",placemark.addressDictionary );
         
         //String to address
         NSString *locatedaddress = [[placemark.addressDictionary valueForKey:@"FormattedAddressLines"] componentsJoinedByString:@", "];
         
         //Print the location in the console
         LocateCustomCell *cell = (LocateCustomCell *)[self.tblLocate cellForRowAtIndexPath:index];
         cell.lblAddress.text = [NSString stringWithFormat:@"@ %@ %@",locatedaddress,[[GlobalClass sharedInstance] ticksToDate:[[[[[arrList objectAtIndex:index.row] objectForKey:@"LastLocs"] objectAtIndex:0] objectForKey:@"TimeStamp"] stringValue]]];
         NSLog(@"Currently address is: %@",locatedaddress);
         
         dispatch_async(dispatch_get_main_queue(), ^{
             // Update the UI
             [KVNProgress dismiss];
         });
     }];
}






- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    return 90;
}
#pragma mark MailCompose Delegate methods
#pragma mark ---

- (void) mailComposeController:(MFMailComposeViewController *)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError *)error
{
    switch (result)
    {
        case MFMailComposeResultCancelled:
            NSLog(@"Mail cancelled");
            break;
        case MFMailComposeResultSaved:
            NSLog(@"Mail saved");
            break;
        case MFMailComposeResultSent:
            NSLog(@"Mail sent");
            break;
        case MFMailComposeResultFailed:
            NSLog(@"Mail sent failure: %@", [error localizedDescription]);
            break;
        default:
            break;
    }
    
    // Close the Mail Interface
    [self dismissViewControllerAnimated:YES completion:NULL];
}

#pragma mark MessageCompose Delegate methods
#pragma mark ---

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
