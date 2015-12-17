//
//  SOSHostViewController.m
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "SOSHostViewController.h"
#import "MyBuddiesViewController.h"
#import "MyGroupsViewController.h"
#import "LocalHelpViewController.h"
#import "MessageCompose.h"
#import <CommonCrypto/CommonCryptor.h>
#import "Encryption.h"

@interface SOSHostViewController ()<ViewPagerDataSource, ViewPagerDelegate>
@property (nonatomic) NSUInteger numberOfTabs;

@property (nonatomic,weak) IBOutlet UIButton* btnSOS;
@property (nonatomic,weak) IBOutlet UIButton* btnTrackMe;
@property (nonatomic,weak) IBOutlet UIButton* btnCapture;

@property (nonatomic,weak) IBOutlet UILabel* lblSOS;
@property (nonatomic,weak) IBOutlet UILabel* lblTrackMe;
@property (nonatomic,weak) IBOutlet UILabel* lblCapture;

@property (nonatomic,weak) IBOutlet UILabel* lblCountDownNumber;
@property (nonatomic,weak) IBOutlet UIView* viewCountDown;

@property (nonatomic , retain) IBOutlet UILabel *lblTag;
@property (nonatomic , retain) IBOutlet UILabel *lblLoadingMsg1;
@property (nonatomic , retain) IBOutlet UILabel *lblLoadingMsg2;
@property (nonatomic , retain) IBOutlet UILabel *lblLoadingMsg3;
@property (nonatomic , retain) IBOutlet UIButton *btnCancelSOS;
@property (nonatomic , retain) IBOutlet UIImageView *imgLocSwitch;
@property (nonatomic , retain) IBOutlet UIButton *btnLocSwitch;

-(IBAction)sosBtnCicked:(UIButton *)sender;
-(IBAction)trackMeBtnCicked:(UIButton *)sender;
-(IBAction)captureBtnCicked:(UIButton *)sender;
-(IBAction)homeBtnCicked:(UIButton *)sender;
-(IBAction)btnLocationSwitchClicked:(UIButton *)sender;
-(IBAction)cancelSOSBtnCicked:(UIButton *)sender;
-(IBAction)tapOnNumberButton:(UIButton *)sender;

@end

@implementation SOSHostViewController

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
    // Do any additional setup after loading the view from its nib.
    [self performSelector:@selector(loadContent) withObject:nil afterDelay:0];
    arrBuddies = [[NSMutableArray alloc] init];
    arrContent = [[NSMutableArray alloc] initWithObjects:@"My Buddies",@"My Groups",@"Get Local Help", nil];
    self.dataSource = self;
    self.delegate = self;
    self.view.backgroundColor = [UIColor clearColor];
    if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
        if([[GlobalClass sharedInstance] is468SizeScreen]){
            [self.viewCountDown setFrame:CGRectMake(0, 20, 320,550)];
        }
        else{
            [self.viewCountDown setFrame:CGRectMake(0, 20, 320,[UIScreen mainScreen].applicationFrame.size.height)];
        }
        [self.view addSubview:self.viewCountDown];
        NSTimer *theTimer = [NSTimer scheduledTimerWithTimeInterval:1 target:self selector:@selector(countdownTracker:) userInfo:nil repeats:YES];
        // Assume a there's a property timer that will retain the created timer for future reference.
        timer = theTimer;
        // Assume there's a property counter that track the number of seconds before count down reach.
        counter = 5;
    }
    else{
        self.btnSOS.selected = YES;
        self.lblSOS.text = @"Stop SOS";
        self.btnTrackMe.selected = YES;
        self.lblTrackMe.text = @"Stop Tracking";
//        [self trackingProcess];
    }
    
    if([[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] && [[[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] boolValue]){
        self.btnTrackMe.selected = YES;
        self.lblTrackMe.text = @"Stop Tracking";
    }
    
    self.imgLocSwitch.layer.cornerRadius = 1.0;
	BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
    if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"] && servicesEnabled){
        self.btnLocSwitch.hidden =YES;
        self.imgLocSwitch.hidden=YES;
    }else{
        self.btnLocSwitch.hidden =NO;
        self.imgLocSwitch.hidden=NO;
        self.btnTrackMe.selected = NO;
        self.lblTrackMe.text = @"Start Tracking";
    }
    
    [self setfontForlabels];
    
    
    
}

-(void)setfontForlabels {
    
    self.lblTag.font = [UIFont fontWithName:@"SegoeUI" size:self.lblTag.font.pointSize];
    self.lblLoadingMsg1.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLoadingMsg1.font.pointSize];
    self.lblLoadingMsg2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLoadingMsg2.font.pointSize];
    self.lblLoadingMsg3.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLoadingMsg3.font.pointSize];
    self.lblSOS.font = [UIFont fontWithName:@"SegoeUI" size:self.lblSOS.font.pointSize];
    self.lblTrackMe.font = [UIFont fontWithName:@"SegoeUI" size:self.lblTrackMe.font.pointSize];
    self.lblCapture.font = [UIFont fontWithName:@"SegoeUI" size:self.lblCapture.font.pointSize];
    self.lblCountDownNumber.font = [UIFont fontWithName:@"SegoeUI" size:self.lblCountDownNumber.font.pointSize];
    self.btnCancelSOS.titleLabel.font = [UIFont fontWithName:@"SegoeUI" size:self.btnCancelSOS.titleLabel.font.pointSize];
}



-(void)startProcess{
    
//    [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
//									  KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
//									  KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
    @try {
        objGlobalClass = [[GlobalClass alloc] init];
        objGlobalClass.PostProtocol = self;
        [self trackProcess];
        
        arrBuddies = [[[DBaseInteraction sharedInstance] getBuddyPhoneNumbers] mutableCopy];
        [self initiateSOSTracking];
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
    
}

-(void)trackProcess{
    @try {
	    BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
        if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"] && servicesEnabled){
        	if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] boolValue]){
				self.btnTrackMe.selected = YES;
            	self.lblTrackMe.text = @"Stop Tracking";
            	[objGlobalClass stopPostingandIndex:0];
            	[[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsTrackingON"];
            	[[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
            	NSString* stringUUID = [[NSUUID UUID] UUIDString];
            	[[NSUserDefaults standardUserDefaults] setObject:stringUUID forKey:@"SessionToken"];
           	    [self postLocationArray];
           	 	[[DBaseInteraction sharedInstance] updateSessionToken:stringUUID andTracking:YES forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
           	    [UIApplication sharedApplication].idleTimerDisabled = YES;
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

-(void)stopSOSEvent{
    if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
        
    }
    else{
    }
}


-(void)stopPostedSuccessfully:(NSString *)successStr{
    [self initiateSOSTracking];
}

-(void)initiateSOSTracking {
    
            startPushpin = YES;
            if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
                NSString* stringUUID = [[NSUUID UUID] UUIDString];
                [[NSUserDefaults standardUserDefaults] setObject:stringUUID forKey:@"SessionToken"];
            }
            [[NSUserDefaults standardUserDefaults] setObject:@"1" forKey:@"TokenId"];
            [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsSosON"];
            [[DBaseInteraction sharedInstance] updateSOS:YES forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
            [UIApplication sharedApplication].idleTimerDisabled = YES;
            
            if([[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
                [objGlobalClass postLocationWithMediaContent:nil];
                startPushpin = NO;
                [self handleAfterPost];
            }
            else{
                [self msgPreparation];
                if(arrBuddies.count>0){
                    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:[NSString stringWithFormat:@"%@",[[arrBuddies objectAtIndex:0] objectForKey:@"PhoneNumber"]] delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"Call", nil];
                    alert.tag = 1;
                    [alert show];
                    
                }
            }
        [[NSUserDefaults standardUserDefaults] synchronize];
}
-(void)setFBPost:(NSString *)msg1{
    [[GlobalClass sharedInstance] postMsgtoFB:msg1 andIndex:0];
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
            
            [objGlobalClass postLocations:arrLoc andIndex:0];
        }
    }
    
}


-(void)PostedWithMediaSuccessfully:(NSString *)successStr{
    
    NSMutableArray *arr = [[NSMutableArray alloc] init];
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    if(arr.count>0){
        
        NSMutableArray *arrLoc  = [[NSMutableArray alloc] init];
        
        for (int i=0; i< arr.count; i++) {
            NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
            
            
            GeoTag *obj = (GeoTag *)[arr objectAtIndex:i];
            [dict setObject:[NSArray arrayWithObjects:obj.Lati, nil] forKey:@"Lat"];
            [dict setObject:[NSArray arrayWithObjects:obj.Longi, nil] forKey:@"Long"];
            [dict setObject:[NSArray arrayWithObjects:obj.Speed, nil] forKey:@"Spd"];
            [dict setObject:[NSArray arrayWithObjects:obj.Altitude, nil] forKey:@"Alt"];
            [dict setObject:[NSArray arrayWithObjects:obj.accuracy, nil] forKey:@"Accuracy"];
            
            NSNumberFormatter * f = [[NSNumberFormatter alloc] init];
            [f setNumberStyle:NSNumberFormatterDecimalStyle];
            NSNumber * myNumber = [f numberFromString:[[GlobalClass sharedInstance] dateToTicks:[NSDate date]]];
            int count = (int)[arr count];
            [dict setObject:[NSArray arrayWithObjects:myNumber, nil] forKey:@"TS"];
            [dict setObject:[NSNumber numberWithInt:count] forKey:@"LocCnt"];
            [dict setObject:[NSArray arrayWithObjects:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"TokenId"]], nil] forKey:@"IsSOS"];
            [dict setObject:[NSString stringWithFormat:@",0,"] forKey:@"GroupID"];
            [dict setObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]] forKey:@"PID"];
//            if(startPushpin)
//                [dict setObject:[NSString stringWithFormat:@"B"] forKey:@"Cmd"];
//            
//            else
//                [dict setObject:[NSString stringWithFormat:@"E"] forKey:@"Cmd"];
//            
            
            if([[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"] ){
                [dict setObject:[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"] forKey:@"id"];
            }
            else{
                [dict setObject:[NSString stringWithFormat:@"0"] forKey:@"id"];
            }
            
            [arrLoc addObject:dict];
        }
        if(arrLoc.count>0){
            [objGlobalClass postLocations:arrLoc andIndex:0];
        }

    }
}

-(void)postedSuccessfully:(NSString *)successStr{
    
}

-(void)handleAfterPost{
    if([[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
        if(arrBuddies.count>0){
            
            UIDevice *device = [UIDevice currentDevice];
            NSLog(@"%@",arrBuddies);
			NSArray *filteredarray;
//            [[arrBuddies objectAtIndex:0] objectForKey:@"PhoneNumber"]
            if(![[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] || [[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] == NULL || [[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] == nil){
                filteredarray = [arrBuddies filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(Name == %@)", [[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"]]];
            }
            else{
                filteredarray = arrBuddies;
            }   
		    if(filteredarray.count>0){
                NSString *cellNameStr = [NSString stringWithFormat:@"%@",[[filteredarray objectAtIndex:0] objectForKey:@"PhoneNumber"]];
                
                if ([[device model] isEqualToString:@"iPhone"] ) {
                    
                    NSString *phoneNumber = [@"telprompt://" stringByAppendingString:cellNameStr];
                    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:phoneNumber]];
                    
                } else {
                    
                    UIAlertView *warning =[[UIAlertView alloc] initWithTitle:@"Call" message:@"Your device doesn't support this feature." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                    [warning show];
                }
                
                [self msgPreparation];
            }
//            if([[[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] isEqualToString:[[arrBuddies objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]]){
//            }
            
            
            
        }
    }
}

-(void)msgPreparation{
    @try {
        if(arrBuddies.count > 0){
            msg =@"";
            if([[GlobalClass sharedInstance] connected]){
                if([[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
                    NSArray *arrProf = [[DBaseInteraction sharedInstance] getProfile];
                    
                    msg = [NSString stringWithFormat:@"%@ %@ needs urgent help at",[[NSUserDefaults standardUserDefaults] objectForKey:@"first_name"],[[arrProf objectAtIndex:0] objectForKey:@"MobileNumber"]];
                    
                    if([[GlobalClass sharedInstance] connected]){
                        
                        NSString *strSampleUrl = [NSString stringWithFormat:@"%@default.aspx?V=2&pr=%@&s=%@",kGuardianPortalUrl,[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"]];
                        
                        
                        
                        strSampleUrl = [strSampleUrl stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
                        NSString *urlString = [NSString stringWithFormat:@"http://tinyurl.com/api-create.php?url=%@",strSampleUrl];
                        
                        NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
                        [request1 setHTTPMethod:@"GET"];
                        [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
                        
                        [NSURLConnection sendAsynchronousRequest:request1
                                                           queue:[[NSOperationQueue alloc] init]
                                               completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                                   if(data){
                                                       NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                                       NSLog(@"%@",jsonString);
                                                       
                                                       dispatch_async(dispatch_get_main_queue(), ^{
                                                           // Update the UI
                                                           msg = [NSString stringWithFormat:@"%@ %@ needs urgent help at %@",[[NSUserDefaults standardUserDefaults] objectForKey:@"first_name"],[[arrProf objectAtIndex:0] objectForKey:@"MobileNumber"],jsonString];
                                                           
                                                           [self showSMS:msg ForRecipents:arrPhone];
                                                           [self setFBPost:msg];
                                                       });
                                                   }
                                                   
                                               }];
                    }
                    
                    else{
                    }
                    
                    
                }
                else{
                    
                    
                    NSString *strMesg;
                    NSMutableArray *arr = [[NSMutableArray alloc] init];
                    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
                    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
                        arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
                    }
                    if(arr.count>0){
                        GeoTag *obj =(GeoTag *)[arr lastObject];
                        strMesg = [NSString stringWithFormat:@"%@default.aspx?V=2&d=%@&l=%@&g=%@",kGuardianPortalUrl,[[GlobalClass sharedInstance] dateToTicks:[NSDate date]],obj.Lati,obj.Longi];
                    }
                    
                    
                    
                    
                    NSString *urlString = [NSString stringWithFormat:@"http://tinyurl.com/api-create.php?url=%@",strMesg];
                    
                    NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
                    [request1 setHTTPMethod:@"GET"];
                    [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
                    
                    [NSURLConnection sendAsynchronousRequest:request1
                                                       queue:[[NSOperationQueue alloc] init]
                                           completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                               if(data){
                                                   NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                                   NSLog(@"%@",jsonString);
                                                   
                                                   dispatch_async(dispatch_get_main_queue(), ^{
                                                       // Update the UI
                                                       
                                                       msg = [NSString stringWithFormat:@"%@ I'm at %@",kMessageTemplateText,jsonString];
                                                       [self showSMS:msg ForRecipents:arrPhone];
                                                       [self setFBPost:msg];
                                                   });
                                               }
                                               
                                           }];
                    
                    
//                    msg = [self offlineUnRegisteredSMStext];
//                    [self showSMS:msg ForRecipents:arrPhone];
//                    [self setFBPost:msg];
                }
            }
            else{
                if([[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
                    msg = [self offlineRegisteredSMStext];
                }
                else{
                    msg = [self offlineUnRegisteredSMStext];
                }
                [self showSMS:msg ForRecipents:arrPhone];
                [self setFBPost:msg];
            }
            
            if(!arrPhone)
                arrPhone = [[NSMutableArray alloc] init];
            else{
                [arrPhone removeAllObjects];
                [arrPhone mutableCopy];
            }
            
            for (int i=0; i<arrBuddies.count; i++) {
                [arrPhone addObject:[[arrBuddies objectAtIndex:i] objectForKey:@"PhoneNumber"]];
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


-(NSString *)offlineRegisteredSMStext{
    NSString *strMesg;
    NSMutableArray *arr = [[NSMutableArray alloc] init];
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    if(arr.count>0){
        //I'm at %@default.aspx?track=%@%@&lat=%@&long=%@&tick=%@
        //,obj.Lati,obj.Longi,[[GlobalClass sharedInstance] dateToTicks:[NSDate date]]
         GeoTag *obj = (GeoTag *)[arr lastObject];
        NSString *sosValue = @"";
        if([[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
            sosValue = [NSString stringWithFormat:@"1"] ;
        }
        else{
            sosValue = [NSString stringWithFormat:@"0"] ;
        }
        NSString *str = [NSString stringWithFormat:@"p=%@&s=%@&f=%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"],sosValue];
        
        NSData *plain = [str dataUsingEncoding:NSUTF8StringEncoding];
        Encryption *cipher =  [[Encryption alloc] init];
        NSError *error;
        NSData *encryptedData = [cipher encryptedDataForData:plain password:nil iv:nil salt:nil error:error];
        NSString *base64 = [[GlobalClass sharedInstance] base64forData:encryptedData];
        plain = [base64 dataUsingEncoding:NSUTF8StringEncoding];
        str = [[GlobalClass sharedInstance] base64forData:plain];
       
        strMesg = [NSString stringWithFormat:@"%@ I'm @ %@default.aspx?V=2&t=%@&ut=%@&d=%@&l=%@&g=%@",kMessageTemplateText,kGuardianPortalUrl,str,[[GlobalClass sharedInstance] utcdateToTicks:[NSDate date]],[[GlobalClass sharedInstance] dateToTicks:[NSDate date]],obj.Lati,obj.Longi];
    }
    
    return strMesg;
}

-(NSString *)offlineUnRegisteredSMStext{
    
    NSString *strMesg;
    NSMutableArray *arr = [[NSMutableArray alloc] init];
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    if(arr.count>0){
        GeoTag *obj =(GeoTag *)[arr lastObject];
        strMesg = [NSString stringWithFormat:@"%@ I'm at %@default.aspx?V=2&d=%@&l=%@&g=%@",kMessageTemplateText,kGuardianPortalUrl,[[GlobalClass sharedInstance] dateToTicks:[NSDate date]],obj.Lati,obj.Longi];
    }

    return strMesg;
}


- (void)countdownTracker:(NSTimer *)theTimer {
    counter--;
    if (counter < 0) {
        [timer invalidate];
        timer = nil;
        counter = 0;
        [self.viewCountDown removeFromSuperview];
        self.btnSOS.selected = YES;
        [self startProcess];
        [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsSosON"];
        self.lblSOS.text = @"Stop SOS";
        [[NSUserDefaults standardUserDefaults] synchronize];
		BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
        if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"] && servicesEnabled){
            self.btnTrackMe.selected = YES;
            self.lblTrackMe.text = @"Stop Tracking";
        }
	}
	    else{
		    self.lblCountDownNumber.text = [NSString stringWithFormat:@"%ld",(long)counter];
	}
}

- (void)viewDidAppear:(BOOL)animated {
    
    [super viewDidAppear:animated];
    
}

#pragma mark - Setters
- (void)setNumberOfTabs:(NSUInteger)numberOfTabs {
    
    // Set numberOfTabs
    _numberOfTabs = numberOfTabs;
    
    // Reload data
    [self reloadData];
    
}

#pragma mark - Helpers
- (void)loadContent {
    self.numberOfTabs = 3;
}

#pragma mark - Interface Orientation Changes
- (void)willRotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation duration:(NSTimeInterval)duration {
    
    // Update changes after screen rotates
    [self performSelector:@selector(setNeedsReloadOptions) withObject:nil afterDelay:duration];
}

#pragma mark - ViewPagerDataSource
- (NSUInteger)numberOfTabsForViewPager:(ViewPagerController *)viewPager {
    return self.numberOfTabs;
}
- (UIView *)viewPager:(ViewPagerController *)viewPager viewForTabAtIndex:(NSUInteger)index {
    
    UILabel *label = [UILabel new];
    label.backgroundColor = [UIColor clearColor];
    label.font = label.font = [UIFont fontWithName:@"SegoeUI" size:15.0];;
    label.text = [NSString stringWithFormat:@"%@",[arrContent objectAtIndex:index]];
    label.textAlignment = NSTextAlignmentCenter;
    label.textColor = [UIColor blackColor];
    [label sizeToFit];
    
    return label;
}

- (UIViewController *)viewPager:(ViewPagerController *)viewPager contentViewControllerForTabAtIndex:(NSUInteger)index {
    UIViewController *vc;
    
    if(index == 0){
        vc = [[MyBuddiesViewController alloc] init];
    }
    else if(index == 1){
        vc = [[MyGroupsViewController alloc] init];
    }else if(index == 2){
        vc = [[LocalHelpViewController alloc] init];
    }
    
    return vc;
}

#pragma mark - ViewPagerDelegate
- (CGFloat)viewPager:(ViewPagerController *)viewPager valueForOption:(ViewPagerOption)option withDefault:(CGFloat)value {
    
    switch (option) {
        case ViewPagerOptionStartFromSecondTab:
            return 0.0;
        case ViewPagerOptionCenterCurrentTab:
            return 0.0;
        case ViewPagerOptionTabLocation:
            return 1.0;
        case ViewPagerOptionTabHeight:
            return 40.0;
        case ViewPagerOptionTabOffset:
            return 20.0;
        case ViewPagerOptionTabWidth:
            return UIInterfaceOrientationIsLandscape(self.interfaceOrientation) ? 128.0 : 96.0;
        case ViewPagerOptionFixFormerTabsPositions:
            return 1.0;
        case ViewPagerOptionFixLatterTabsPositions:
            return 0.0;
        default:
            return value;
    }
}
- (UIColor *)viewPager:(ViewPagerController *)viewPager colorForComponent:(ViewPagerComponent)component withDefault:(UIColor *)color {
    
    switch (component) {
        case ViewPagerIndicator:
            return [[UIColor redColor] colorWithAlphaComponent:0.64];
        case ViewPagerTabsView:
            return [[UIColor lightGrayColor] colorWithAlphaComponent:0.32];
        case ViewPagerContent:
            return [[UIColor darkGrayColor] colorWithAlphaComponent:0.32];
        default:
            return color;
    }
}

#pragma mark IBAction Methods 
#pragma mark ---- 

-(IBAction)sosBtnCicked:(UIButton *)sender{
    if(self.btnSOS.selected)   {
        self.btnSOS.selected = NO;
        self.lblSOS.text = @"Start SOS";
        [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"IsSosON"];
        [UIApplication sharedApplication].idleTimerDisabled = NO;
        [[NSUserDefaults standardUserDefaults] synchronize];
        [[DBaseInteraction sharedInstance] updateSOS:NO forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
        [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
        [self showSMS:[NSString stringWithFormat:@"I'm safe now. Thanks for the help! I will get in touch with you shortly and share the details."] ForRecipents:arrPhone];
        [self setFBPost:[NSString stringWithFormat:@"I'm safe now. Thanks for the help! I will get in touch with you shortly and share the details."]];
        [[GlobalClass sharedInstance] stopSOSandIndex:0];
    }
    else {
        self.btnSOS.selected = YES;
        self.lblSOS.text = @"Stop SOS";
		BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
        if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"] && servicesEnabled){
            self.btnTrackMe.selected = YES;
            self.lblTrackMe.text = @"Stop Tracking";
		 }
		 [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsSosON"];
        [[NSUserDefaults standardUserDefaults] setObject:@"1" forKey:@"TokenId"];
        [UIApplication sharedApplication].idleTimerDisabled = YES;
        [[DBaseInteraction sharedInstance] updateSOS:NO forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
        [[NSUserDefaults standardUserDefaults] synchronize];
        [self startProcess];
    }
}
-(IBAction)trackMeBtnCicked:(UIButton *)sender{
    @try {
        if(sender.selected)   {
            
            if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
                
                sender.selected = NO;
                self.lblTrackMe.text = @"Start Tracking";
                
                [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"IsTrackingON"];
                [UIApplication sharedApplication].idleTimerDisabled = NO;
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
        else  {
            sender.selected = YES;
            self.lblTrackMe.text = @"Stop Tracking";
            [self trackProcess];
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
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}
-(IBAction)homeBtnCicked:(UIButton *)sender{
    [self.navigationController popViewControllerAnimated:YES];
}

-(IBAction)btnLocationSwitchClicked:(UIButton *)sender{
    @try {
        
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
        }else{
            [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"LocationConsent"];;
            [[DBaseInteraction sharedInstance]  updatetLocationConsent:YES forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
            self.btnLocSwitch.hidden =YES;
            self.imgLocSwitch.hidden=YES;
            
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
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Successfully location consent enabled !" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil];
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


#pragma mark UIImagePickerDelegate Methods
#pragma mark ----

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingImage:(UIImage *)image editingInfo:(NSDictionary *)editingInfo{
    
    [picker dismissViewControllerAnimated:NO completion:nil];
    if(self.btnSOS.selected){
	int width = image.size.width;
    int height = image.size.height;
    if (width > 1000 || height > 1000)
    {
        width = width / 2;
        height =  height / 2 ;
    }
        UIImage *img = [[GlobalClass sharedInstance] imageWithImage:image scaledToSize:CGSizeMake(200, 200)];
        NSData* imageData = UIImageJPEGRepresentation(img, 0);
        
        const unsigned char *bytes = [imageData bytes];
        NSUInteger length = [imageData length];
        NSMutableArray *byteArray = [NSMutableArray array];
        for (NSUInteger i = 0; i < length; i++)
        {
            [byteArray addObject:[NSNumber numberWithUnsignedChar:bytes[i]]];
        }
        
        [[GlobalClass sharedInstance] postLocationWithMediaContent:byteArray];
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


#pragma mark CountDown Screen 
#pragma mark 

-(IBAction)cancelSOSBtnCicked:(UIButton *)sender{
    [self.viewCountDown removeFromSuperview];
	[timer invalidate];
    [self.navigationController popViewControllerAnimated:YES];
}
-(IBAction)tapOnNumberButton:(UIButton *)sender{
    // Call
    [self startProcess];
	[timer invalidate];
    [self.viewCountDown removeFromSuperview];
    [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsSosON"];
    self.lblSOS.text = @"Stop SOS";
    [[NSUserDefaults standardUserDefaults] synchronize];
    self.btnSOS.selected = YES;
	BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
    if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"] && servicesEnabled){
    self.btnTrackMe.selected = YES;
    self.lblTrackMe.text = @"Stop Tracking";
	}
}


- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark MessageCompose Delegate methods
#pragma mark --- 

-(void)showSMS:(NSString*)message ForRecipents:(NSArray *)recipents{
    
    @try {
        if(![[[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"] stringValue] isEqualToString:@"0"]){
            NSArray *arr = [[DBaseInteraction sharedInstance] getAllowancesForProfiles:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
            NSLog(@"%@",arr);
            if(arr.count>0){
                if(![[[arr objectAtIndex:0] valueForKey:@"CanSMS"] boolValue]){
                    
                    return;
                }
            }
            else return;
        }
        
        
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

#pragma mark UIAlertView Delegate methods

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
    if(alertView.tag==1){
        if(buttonIndex == 1){
            UIDevice *device = [UIDevice currentDevice];
        
            NSString *cellNameStr = [NSString stringWithFormat:@"%@",[[arrBuddies objectAtIndex:0] objectForKey:@"PhoneNumber"]];
        
            if ([[device model] isEqualToString:@"iPhone"] ) {
                
                NSString *phoneNumber = [@"telprompt://" stringByAppendingString:cellNameStr];
                [[UIApplication sharedApplication] openURL:[NSURL URLWithString:phoneNumber]];
                
            } else {
                
                UIAlertView *warning =[[UIAlertView alloc] initWithTitle:@"Call" message:@"Your device doesn't support this feature." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                
                [warning show];
            }
        }
        else{
           // [self showSMS:msg ForRecipents:arrPhone];
        }

    }
}


@end
