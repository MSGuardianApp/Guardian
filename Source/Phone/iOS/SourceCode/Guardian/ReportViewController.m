//
//  ReportViewController.m
//  Guardian
//
//  Created by PTG on 19/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "ReportViewController.h"
#import "RegisterViewController.h"
#import "RadioButton.h"



@interface ReportViewController ()
@property (nonatomic , weak) IBOutlet UIView *viewMicrosoftTile;
@property (weak, nonatomic) IBOutlet UITextView *txtDescriptionView;
- (IBAction)ReportButtonAction:(id)sender;
//@property (strong, nonatomic) IBOutletCollection(DLRadioButton) NSArray *topRadioButtons;
@property (weak, nonatomic) IBOutlet RadioButton *radioBtn;
@property (weak, nonatomic) IBOutlet UIButton *btnReport;
//@property (nonatomic, strong) IBOutlet RadioButton* radioButton;
@property (weak, nonatomic) IBOutlet UIScrollView *scrView;
- (IBAction)RadioButtonAction:(RadioButton *)sender;

@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect1;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect2;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect3;

@property (nonatomic , retain) IBOutlet UILabel *lblIncidentsTitle;
@property (nonatomic , retain) IBOutlet UILabel *lblIncident1;
@property (nonatomic , retain) IBOutlet UILabel *lblIncident2;
@property (nonatomic , retain) IBOutlet UILabel *lblIncident3;
@property (nonatomic , retain) IBOutlet UILabel *lblIncident4;
@property (nonatomic , retain) IBOutlet UILabel *lblIncident5;
@property (nonatomic , retain) IBOutlet UILabel *lblDescriptionTitle;
@property (nonatomic , retain) IBOutlet UILabel *lblmsg1;
@property (nonatomic , retain) IBOutlet UILabel *lblmsg2;
@property (nonatomic , retain) IBOutlet UIButton *btnPrivacy;


@property(strong,nonatomic)NSMutableArray *arrayOfAllRadioButtons;

-(IBAction)btnBackClicked:(id)sender;
-(IBAction)btnPrivacyPoilcyClicked:(id)sender;
@end

@implementation ReportViewController
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
    [self.scrView setContentSize:CGSizeMake(320,[UIScreen mainScreen].applicationFrame.size.height-65)];
    
    lat = @"";
    longi = @"";
    
    incidentStr = @"HARASSMENT";
    
    self.btnReport.layer.borderWidth =1.5f;
    self.btnReport.layer.borderColor = [UIColor whiteColor].CGColor;
    
    NSLog(@"%@",self.viewMicrosoftTile);
    
    UITapGestureRecognizer * tapGesture = [[UITapGestureRecognizer alloc]
                                           initWithTarget:self
                                           action:@selector(hideKeyBoard)];
    
    [self.view addGestureRecognizer:tapGesture];
    [self setfontForlabels];
}

-(void)viewWillAppear:(BOOL)animated{
    [self updateView];
}

-(void)updateView{
    
    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
        [self.viewMicrosoftTile setFrame:CGRectMake(0, 80, 320,[UIScreen mainScreen].applicationFrame.size.height-65)];
        [self.view addSubview:self.viewMicrosoftTile];
        //       [self.scrView removeFromSuperview];
    }
    else{
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        if([defaults boolForKey:@"ProfileInserted"]){
            [self.scrView setFrame:CGRectMake(0, 65, self.scrView.frame.size.width,self.scrView.frame.size.height)];
            [self.view addSubview:self.scrView];
            [self.viewMicrosoftTile removeFromSuperview];
        }
        else{
            [self.viewMicrosoftTile setFrame:CGRectMake(0, 80, 320,[UIScreen mainScreen].applicationFrame.size.height-65)];
            [self.view addSubview:self.viewMicrosoftTile];
            //            [self.scrView removeFromSuperview];
        }
    }
    [self updateLocation];

}

-(void)setfontForlabels {
    
    self.lblIncidentsTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblIncidentsTitle.font.pointSize];
    self.lblIncident1.font = [UIFont fontWithName:@"SegoeUI" size:self.lblIncident1.font.pointSize];
    self.lblIncident2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblIncident2.font.pointSize];
    self.lblIncident3.font = [UIFont fontWithName:@"SegoeUI" size:self.lblIncident3.font.pointSize];
    self.lblIncident4.font = [UIFont fontWithName:@"SegoeUI" size:self.lblIncident4.font.pointSize];
    self.lblIncident5.font = [UIFont fontWithName:@"SegoeUI" size:self.lblIncident5.font.pointSize];
    
    self.lblDescriptionTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblDescriptionTitle.font.pointSize];
    self.lblmsg1.font = [UIFont fontWithName:@"SegoeUI" size:self.lblmsg1.font.pointSize];
    self.lblmsg2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblmsg2.font.pointSize];
    self.btnReport.titleLabel.font = [UIFont fontWithName:@"SegoeUI" size:self.btnReport.titleLabel.font.pointSize];
    self.btnPrivacy.titleLabel.font = [UIFont fontWithName:@"SegoeUI" size:self.btnPrivacy.titleLabel.font.pointSize];
    
    self.lblLiveConnect1.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect1.font.pointSize];
    self.lblLiveConnect2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect2.font.pointSize];
    self.lblLiveConnect3.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect3.font.pointSize];
    
    self.txtDescriptionView.font = [UIFont fontWithName:@"SegoeUI" size:self.txtDescriptionView.font.pointSize];
}

-(void)updateLocation{
    NSMutableArray *arr= [[NSMutableArray alloc] init];
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    
    if(arr.count>0){
        
        GeoTag *obj = (GeoTag *)[arr lastObject];
        lat = obj.Lati;
        longi = obj.Longi;
        NSLog(@"%@  %@",lat , longi);
        
    }
}


- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}
+ (NSURLSession *)sessionObj
{
    static NSURLSession *session = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        
        NSURLSessionConfiguration *configuration = [NSURLSessionConfiguration defaultSessionConfiguration];
        
        [configuration setHTTPMaximumConnectionsPerHost:1];
        
        session = [NSURLSession sessionWithConfiguration:configuration];
        
    });
    return session;
}

- (IBAction)ReportButtonAction:(id)sender {
    if(lat.length > 0){
        
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
    
}

-(IBAction)btnPrivacyPoilcyClicked:(id)sender{
    NSString* launchUrl = @"https://guardianportal.cloudapp.net/privacy.htm";
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString: launchUrl]];
}



- (IBAction)RadioButtonAction:(RadioButton *)sender {
    
    switch(sender.tag)
    {
        case 1:
            incidentStr = @"HARASSMENT";
            break;
            
        case 2:
            incidentStr = @"RAGGING";
            break;
            
        case 3:
            incidentStr = @"ACCIDENT";
            break;
            
        case 4:
            incidentStr = @"ROBBERY";
            break;
            
        case 5:
            incidentStr = @"OTHERS";
            break;
            
        default:
            break;
    }
}

- (IBAction)onClickSignInButton:(id)sender
{
    MicrosoftLiveConnect *obj = [[MicrosoftLiveConnect alloc] init];
    [self.navigationController pushViewController:obj animated:NO];
}

-(IBAction)btnBackClicked:(id)sender{
    [self.navigationController popViewControllerAnimated:YES];
}



#pragma mark UIImagePickerDelegate Methods 
#pragma mark ----

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingImage:(UIImage *)image editingInfo:(NSDictionary *)editingInfo{
    [self.txtDescriptionView resignFirstResponder];
    [picker dismissViewControllerAnimated:NO completion:nil];
    
    @try {
        int width = image.size.width;
        int height = image.size.height;
        if (width > 1000 || height > 1000)
        {
            width = width / 2;
            height =  height / 2 ;
        }
        
        UIImage *img = [[GlobalClass sharedInstance] imageWithImage:image scaledToSize:CGSizeMake(width, height)];
        [self updateReportTease:img andIndex:0];
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}
-(void)updateReportTease:(UIImage *)image andIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        if([[GlobalClass sharedInstance] connected]){
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            
            NSData* imageData = UIImageJPEGRepresentation(image, 0);
            
            
            const unsigned char *bytes = [imageData bytes];
            NSUInteger length = [imageData length];
            NSMutableArray *byteArray = [NSMutableArray array];
            for (NSUInteger i = 0; i < length; i++)
            {
                [byteArray addObject:[NSNumber numberWithUnsignedChar:bytes[i]]];
            }
            
//            if(byteArray.count>0){
//                AppDelegate *app = (AppDelegate *)[[UIApplication sharedApplication] delegate];
//                [app foundLocation:app.locationManager.location];
//            }
            
            NSString *name = @"";
            NSString *mobileNo = @"";
            NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
            if([defaults boolForKey:@"ProfileInserted"]){
                NSArray *arr = [[DBaseInteraction sharedInstance] getProfile];
                NSLog(@"%@",arr);
                if(arr.count > 0){
                    name = [defaults objectForKey:@"name"];
                    mobileNo = [[arr objectAtIndex:0] objectForKey:@"MobileNumber"];
                }
            }
            
            NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
            [dict setObject:name forKey:@"Name"];
            [dict setObject:mobileNo forKey:@"MobileNumber"];
            [dict setObject:self.txtDescriptionView.text forKey:@"AdditionalInfo"];
            [dict setObject:@"0.0" forKey:@"Alt"];
            [dict setObject:incidentStr forKey:@"Command"];
            [dict setObject:@"0" forKey:@"Identifier"];
            [dict setObject:@"1" forKey:@"GeoDirection"];
            [dict setObject:lat forKey:@"Lat"   ];
            [dict setObject:longi forKey:@"Long"];
            [dict setObject:byteArray forKey:@"MediaContent"];
            [dict setObject:@"" forKey:@"MediaUri"];
            [dict setObject:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"] forKey:@"ProfileID"];
            [dict setObject:@"0" forKey:@"Speed"];
            [dict setObject:[[GlobalClass sharedInstance] dateToTicks:[NSDate date]] forKey:@"TimeStamp"];
            
            
            NSData *requestData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:nil];
            
            
            
            NSURL * url = [NSURL URLWithString:[NSString stringWithFormat:@"%@",kReportTeaseUrl]];
            
            NSMutableURLRequest * urlRequest = [NSMutableURLRequest requestWithURL:url];
            NSString *params = [[NSString alloc] initWithData:requestData encoding:NSUTF8StringEncoding];
            
            NSLog(@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"]);
            
            
            [urlRequest setHTTPMethod:@"POST"];
            [urlRequest setHTTPBody:[params dataUsingEncoding:NSUTF8StringEncoding]];
            [urlRequest setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
            [urlRequest setValue: @"application/json" forHTTPHeaderField: @"Accept"];
            [urlRequest setValue: @"application/json" forHTTPHeaderField: @"Content-Type"];
            
            [NSURLConnection sendAsynchronousRequest:urlRequest
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                       if(data && !error){
                                           NSString *requestStr = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                           NSLog(@"%@",requestStr);
                                           
                                           
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               // Update the UI
                                               [KVNProgress dismiss];
                                               UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Evidence has been captured and uploaded to the guardian server !" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil];
                                               [alert show];
                                           });
                                       }
                                       else{
                                           [self updateReportTease:image andIndex:(NoOfTimes+1)];
                                       }
                                       
                                   }];
        }
        else{
//            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//            [alert show];
        }
    }
}


- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
    [self.navigationController popViewControllerAnimated:YES];
}
#pragma mark TextField delegate methods

-(void)hideKeyBoard{
    for(UITextField *t in self.scrView.subviews){
        [t resignFirstResponder];
        [self.scrView setContentOffset:CGPointMake(0, 0) animated:YES];
    }
}

- (void)keyboardDidShow: (NSNotification *) notif{
    [self.scrView setContentSize:CGSizeMake(self.scrView.frame.size.width, 620)];
}
- (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString *)text;
{
    //if ( [textView == YourTextField] ) {
        if ([text isEqualToString:@"\n"]) {
            [textView resignFirstResponder];
            return NO;
       // }
    }
    return YES;
}

- (void)textViewDidBeginEditing:(UITextView *)textView{
    [self.scrView setContentOffset:CGPointMake(0, 150) animated:YES];
}
- (void)textViewDidEndEditing:(UITextView *)textView{
    [self.scrView setContentOffset:CGPointMake(0, 0) animated:YES];
}

@end
