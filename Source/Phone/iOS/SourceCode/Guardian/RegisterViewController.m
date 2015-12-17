//
//  RegisterViewController.m
//  Guardian
//
//  Created by PTG on 18/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "RegisterViewController.h"
#import "PhoneSetting.h"
#import "SiteSetting.h"
#import "NBPhoneNumber.h"
#import "TPKeyboardAvoidingScrollView.h"

@interface RegisterViewController ()
@property (weak, nonatomic) IBOutlet UITextField *txtNameField;
@property (weak, nonatomic) IBOutlet UITextField *txtAccountField;
@property (weak, nonatomic) IBOutlet UITextField *txtNumberField;
@property (weak, nonatomic) IBOutlet UITextField *txtSecurityCode;
@property (weak, nonatomic) IBOutlet UITextField *txtRegionalCode;
@property (weak, nonatomic) IBOutlet UITextField *txtOrgEmailId;
@property (weak, nonatomic) IBOutlet UITextField *txtEmailSecurityCode;
@property (weak, nonatomic) IBOutlet UILabel *lblActionTitle;
@property (weak, nonatomic) IBOutlet UILabel *lblOrgEmailId;
@property (weak, nonatomic) IBOutlet UILabel *lblEmailSecurityCode;
@property (weak, nonatomic) IBOutlet UIButton *btnValidate;
@property (weak, nonatomic) IBOutlet UIButton *btnConfirm;
@property (weak, nonatomic) IBOutlet TPKeyboardAvoidingScrollView *scrlView;
@property (weak, nonatomic) IBOutlet UIView *viewValidate;
@property (weak, nonatomic) IBOutlet UIView *viewConfirm;
- (IBAction)validateButtonAction:(id)sender;
- (IBAction)ComfirmButtonAction:(id)sender;
- (IBAction)regionCodeButtonAction:(id)sender;
@property (weak, nonatomic) IBOutlet UIButton *seeCharButtonAction;
@property (weak, nonatomic) IBOutlet UIButton *seeEmailCharButtonAction;

@end

@implementation RegisterViewController

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
    [self.scrlView contentSizeToFit];
    
    self.btnConfirm.layer.borderWidth =1.5f;
    self.btnConfirm.layer.borderColor = [UIColor whiteColor].CGColor;
    self.btnValidate.layer.borderWidth =1.5f;
    self.btnValidate.layer.borderColor =[UIColor whiteColor].CGColor;
    
    [self.scrlView setContentSize:CGSizeMake(self.scrlView.frame.size.width,650)];
    
    isEdited = NO;
    
    if(!isEnterpriseUser){
        self.lblOrgEmailId.hidden = YES;
        self.txtOrgEmailId.hidden = YES;
        self.lblEmailSecurityCode.hidden = YES;
        self.txtEmailSecurityCode.hidden = YES;
        self.seeEmailCharButtonAction.hidden = YES;
        
        [self.viewValidate setFrame:CGRectMake(self.viewValidate.frame.origin.x, self.viewValidate.frame.origin.y-72, self.viewValidate.frame.size.width, self.viewValidate.frame.size.height)];
        
        [self.viewConfirm setFrame:CGRectMake(self.viewConfirm.frame.origin.x, self.viewConfirm.frame.origin.y-61, self.viewConfirm.frame.size.width, self.viewConfirm.frame.size.height)];
        [self.scrlView setContentSize:CGSizeMake(self.scrlView.frame.size.width,520)];
    }
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(keyboardDidShow:)
                                                 name:UIKeyboardDidShowNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(keyboardDidHide:)
                                                 name:UIKeyboardDidHideNotification
                                               object:nil];

    
//    countryXml *objXml = [countryXml sharedInstance];
//    objXml.countryXmlDelegate = self;
//    @try {
//        [objXml parseCountryXmlFile];
//    }
//    @catch (NSException *exception) {
//        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
//        NSLog(@"%@",exception);
//    }
//    @finally {
//    }
    
    arrCountryList = [[NSMutableArray alloc] init];
    arrCountryList = [DBaseInteraction sharedInstance].arrCountries;
    
    NSArray *filteredarray = [arrCountryList filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(IsdCode == %@)", [[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"]]];
    if(filteredarray.count>0){
        maxPhoneDigit = [[[filteredarray objectAtIndex:0] objectForKey:@"MaxPhoneDigits"] integerValue];
    }
    
    [self intializePicker];
    
    
    strPhoneNumber = @"";
//    maxPhoneDigit = 10;
    self.txtRegionalCode.text = [[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"];
    UITapGestureRecognizer * tapGesture = [[UITapGestureRecognizer alloc]
                                           initWithTarget:self
                                           action:@selector(hideKeyBoard)];
    
    [self.scrlView addGestureRecognizer:tapGesture];
    
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    self.txtNameField.text = [[NSUserDefaults standardUserDefaults] objectForKey:@"name"];
    self.txtAccountField.text = [[NSUserDefaults standardUserDefaults] objectForKey:@"email"];
    if(self.isEdit){
//        self.Phonetxt = [self.Phonetxt stringByReplacingOccurrencesOfString:self.txtRegionalCode.text withString:@""];
        self.txtNumberField.text = [self.Phonetxt stringByReplacingOccurrencesOfString:self.txtRegionalCode.text withString:@""];
        self.lblActionTitle.text = @"Update Mobile Number";
        prevRegCode = self.txtRegionalCode.text;
    }
    [defaults synchronize];
    // Do any additional setup after loading the view from its nib.
}

-(void)countryXmlParsedData:(NSMutableArray *)arrData{
    arrCountryList = [[NSMutableArray alloc] init];
    arrCountryList = arrData;
    
    NSArray *filteredarray = [arrCountryList filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(IsdCode == %@)", [[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"]]];
    if(filteredarray.count>0){
        maxPhoneDigit = [[[filteredarray objectAtIndex:0] objectForKey:@"MaxPhoneDigits"] integerValue];
    }
    
    [self intializePicker];
    NSLog(@"%@",arrCountryList);
}

-(void)viewWillDisappear:(BOOL)animated{
    [super viewWillDisappear:YES];
    NSNotificationCenter *center = [NSNotificationCenter defaultCenter];
    [center removeObserver:self name:UIKeyboardWillShowNotification object:nil];
    [center removeObserver:self name:UIKeyboardWillHideNotification object:nil];
}
-(void)intializePicker {
    if(arrCountryList.count>0){
        CGSize iOSDeviceScreenSize = [[UIScreen mainScreen] bounds].size;
        
        
        regionPicker = [[UIPickerView alloc] initWithFrame:CGRectMake(0.0, 44.0, iOSDeviceScreenSize.width, 200.0)];
        regionPicker.delegate =self;
        regionPicker.tag = 1;
        regionPicker.dataSource = self;
        regionPicker.backgroundColor = [UIColor whiteColor];
        
        NSString *title = UIDeviceOrientationIsLandscape([UIDevice currentDevice].orientation) ? @"\n\n\n\n\n\n\n\n\n" : @"\n\n\n\n\n\n\n\n\n\n\n\n" ;
        if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_7_1){
            
            //Pre iOS 8
            Region_Actionsheet = [[UIActionSheet alloc]
                                initWithTitle:title
                                delegate:self
                                cancelButtonTitle:nil
                                destructiveButtonTitle:nil
                                otherButtonTitles: nil];
            
            [Region_Actionsheet addSubview:regionPicker];
            [Region_Actionsheet setTag:1];
            
            
            UIToolbar *pickerDateToolbar = [[UIToolbar alloc] initWithFrame:CGRectMake(0, 0, iOSDeviceScreenSize.width, 44)];
            pickerDateToolbar.barStyle = UIBarStyleDefault;
            pickerDateToolbar.layer.cornerRadius = 10.0;
            [pickerDateToolbar sizeToFit];
            
            NSMutableArray *barItems = [[NSMutableArray alloc] init];
            
            
            UIBarButtonItem *cancelBtn = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemCancel target:self action:@selector(PickerCancelClick:)];
            [barItems addObject:cancelBtn];
            
            UIBarButtonItem *flexibleItem = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace target:nil action:nil];
            [barItems addObject:flexibleItem];
            
            UIBarButtonItem *doneBtn = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemDone target:self action:@selector(PickerDoneClick:)];
            
            [barItems addObject:doneBtn];
            
            
            [pickerDateToolbar setItems:barItems animated:NO];
            
            
            [Region_Actionsheet addSubview:pickerDateToolbar];
            
        } else {
            
            //for iOS 8
            
            regionPickerContainer = [UIAlertController alertControllerWithTitle: title
                                                                      message:nil
                                                               preferredStyle: UIAlertControllerStyleActionSheet];
            regionPickerContainer.modalInPopover = YES;
            [regionPickerContainer.view addSubview:regionPicker];
            
            //Add autolayout constraints to position the datepicker
            [regionPicker setTranslatesAutoresizingMaskIntoConstraints:NO];
            
            // Create a dictionary to represent the view being positioned
            NSDictionary *labelViewDictionary = NSDictionaryOfVariableBindings(regionPicker);
            
            NSArray* hConstraints = [NSLayoutConstraint constraintsWithVisualFormat:@"H:|-[regionPicker]-|" options:0 metrics:nil views:labelViewDictionary];
            [regionPickerContainer.view addConstraints:hConstraints];
            NSArray* vConstraints = [NSLayoutConstraint constraintsWithVisualFormat:@"V:|-[regionPicker]" options:0 metrics:nil views:labelViewDictionary];
            [regionPickerContainer.view addConstraints:vConstraints];
            
            
            [regionPickerContainer addAction:[UIAlertAction actionWithTitle:@"Cancel" style:UIAlertActionStyleDefault handler:^(UIAlertAction* action){
                [self pickerCancelClicked];
            }]];
            
            [regionPickerContainer addAction:[UIAlertAction actionWithTitle:@"OK" style:UIAlertActionStyleDefault handler:^(UIAlertAction* action){
                [self pickerSelected];
            }]];
            
            
        }
    }
}
-(void)PickerCancelClick:(id)sender{
    self.txtRegionalCode.text = prevString;
    [Region_Actionsheet dismissWithClickedButtonIndex:0 animated:YES];
    [self.scrlView setContentOffset:CGPointMake(0, 0) animated:YES];
}
-(void)PickerDoneClick:(id)sender{
    [self.scrlView setContentOffset:CGPointMake(0, 0) animated:YES];
    if(selIndex>=0){
        self.txtRegionalCode.text = [[arrCountryList objectAtIndex:selIndex] objectForKey:@"IsdCode"];
        
        
        maxPhoneDigit = [[[arrCountryList objectAtIndex:selIndex] objectForKey:@"MaxPhoneDigits"] integerValue];
        [Region_Actionsheet dismissWithClickedButtonIndex:selIndex animated:YES];
        return;
    }
    [Region_Actionsheet dismissWithClickedButtonIndex:0 animated:YES];
}
-(void)pickerSelected{
    self.txtRegionalCode.text = [[arrCountryList objectAtIndex:[regionPicker selectedRowInComponent:0]] objectForKey:@"IsdCode"];
    maxPhoneDigit = [[[arrCountryList objectAtIndex:[regionPicker selectedRowInComponent:0]] objectForKey:@"MaxPhoneDigits"] integerValue];
}

-(void)pickerCancelClicked{
    self.txtRegionalCode.text = prevString;
    [self.scrlView setContentOffset:CGPointMake(0, 0) animated:YES];
    [regionPickerContainer dismissViewControllerAnimated:YES completion:nil];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (IBAction)backButtonAction:(id)sender {
    if(isEdited){
        [[NSUserDefaults standardUserDefaults] setObject:self.Phonetxt forKey:@"PhoneNumber"];
        self.txtRegionalCode.text = prevRegCode;
        self.txtNumberField.text = [self.Phonetxt stringByReplacingOccurrencesOfString:self.txtRegionalCode.text withString:@""];
        NSArray *filteredarray = [arrCountryList filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(IsdCode == %@)", [[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"]]];
        if(filteredarray.count>0){
            maxPhoneDigit = [[[filteredarray objectAtIndex:0] objectForKey:@"MaxPhoneDigits"] integerValue];
        }
        [[NSUserDefaults standardUserDefaults] setObject:self.txtRegionalCode.text forKey:@"RegionCode"];
    }
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (IBAction)regionCodeButtonAction:(id)sender{
       if(!regionPicker){
            [self intializePicker];
        }
        
        if(arrCountryList.count>0){
            prevString = self.txtRegionalCode.text;
            if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_7_1){
                
                //Pre iOS 8
//                [Region_Actionsheet showFromRect:CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200) inView:self.view animated:YES];
                
                [Region_Actionsheet showInView:self.scrlView];
                
            } else {
                
                //for iOS 8
                
                [self presentViewController:regionPickerContainer animated:YES completion:nil];
                
            }
            [self.scrlView setContentOffset:CGPointMake(0, 100) animated:YES];
        }
//        else{
//            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"Please add names in settings" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil];
//            [alert show];
//        }
}

- (IBAction)validateButtonAction:(id)sender {
    
    @try {
        BOOL isValid = YES;
        
        [self hideKeyBoard];
        
        if(isEnterpriseUser){
        
            if(self.txtOrgEmailId.text == nil || [[self.txtOrgEmailId.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] isEqualToString:@""]){
                isValid = NO;
            }
            else{
                if([[GlobalClass sharedInstance] emailValidation:self.txtEmailSecurityCode.text]){
                    NSString *str = [[self.txtOrgEmailId.text componentsSeparatedByString:@"@"] objectAtIndex:0];
                    str = [NSString stringWithFormat:@"@%@",str];
                    if([str isEqualToString:kEnterpriseDomain]){
                        isValid = YES;
                    }
                    else{
                        isValid = NO;
                    }
                }
                else{
                    isValid = NO;
                }
            }
        }
        if(!isValid){
            UIAlertView *alertView=[[UIAlertView alloc]initWithTitle:@"Guardian" message:@"Please enter valid org EmailId" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
            [alertView show];
            return;
        }
        
        NSString *cellNameStr = self.txtNumberField.text;
        cellNameStr =  [[cellNameStr componentsSeparatedByCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] componentsJoinedByString:@""];
        
        // Create character set with specified characters
        NSMutableCharacterSet *characterSet =
        [NSMutableCharacterSet characterSetWithCharactersInString:@"()-"];
        
        // Build array of components using specified characters as separtors
        NSArray *arrayOfComponents = [cellNameStr componentsSeparatedByCharactersInSet:characterSet];
        
        // Create string from the array components
        cellNameStr = [arrayOfComponents componentsJoinedByString:@""];
        
        cellNameStr = [cellNameStr stringByReplacingOccurrencesOfString:self.txtRegionalCode.text withString:@""];
        
        if(cellNameStr.length == 11 && [cellNameStr characterAtIndex:0] == '0'){
            [cellNameStr stringByReplacingCharactersInRange:NSMakeRange(0, 1) withString:@""];
        }
        
        NBPhoneNumberUtil *phoneUtil = [[NBPhoneNumberUtil alloc] init];
        NSError *error = nil;
        
//        self.txtRegionalCode.text
//        [[NSUserDefaults standardUserDefaults] objectForKey:@"LocaleCode"]
        
        NSNumber *num1 = @([self.txtRegionalCode.text integerValue]);
        
        NBPhoneNumber *phoneNumberUS = [phoneUtil parse:cellNameStr defaultRegion:[phoneUtil getRegionCodeForCountryCode:num1] error:&error];
        if (error) {
            NSLog(@"err [%@]", [error localizedDescription]);
        }
        
        if([phoneUtil isValidNumberForRegion:phoneNumberUS regionCode:[phoneUtil getRegionCodeForCountryCode:num1]]){
            [self validateMobileNumber:cellNameStr andIndex:0];
        }
        else{
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Please Enter Valid Mobile Number" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
        }
        
        
//        if(cellNameStr.length ==maxPhoneDigit){
//            
//            [self validateMobileNumber:cellNameStr andIndex:0];
//            
//        }
//		else{
//            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Please Enter Valid Mobile Number" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
//            [warningAlert show];
//        }
        

    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}

-(void)validateMobileNumber:(NSString *)mobileNumber andIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        if([[GlobalClass sharedInstance] connected]){
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            
            NSString *str = [NSString stringWithFormat:@"%@%@",self.txtRegionalCode.text,mobileNumber];
            strPhoneNumber = mobileNumber;
            
            //        LiveDetails *objLiveDetails = (LiveDetails *)[[NSUserDefaults standardUserDefaults] objectForKey:@"LiveDetails"];
            //        NSLog(@"%@",objLiveDetails.authenticationToken);
            
            
            
            
            //,\"DataInfo\":\"null\"
            NSString *requestStr=[NSString stringWithFormat:@"{\"AuthenticatedLiveID\":\"%@\",\"Name\":\"%@\",\"PhoneNumber\":\"%@\",\"RegionCode\":\"%@\",\"EnterpriseEmailID\":\"%@\"}",[[NSUserDefaults standardUserDefaults] objectForKey:@"email"],[[NSUserDefaults standardUserDefaults] objectForKey:@"name"],str,self.txtRegionalCode.text,self.txtOrgEmailId.text];
            NSLog(@"JSON summary: %@", requestStr);
            
            
            
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:[NSString stringWithFormat:@"%@",kPhoneValidatorUrl]]
                                                                    cachePolicy: NSURLRequestUseProtocolCachePolicy
                                                                timeoutInterval: 60.0];
            [request1 setHTTPMethod:@"POST"];
            [request1 setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
            [request1 setValue: @"application/json" forHTTPHeaderField: @"Accept"];
            [request1 setValue: @"application/json" forHTTPHeaderField: @"Content-Type"];
            [request1 setHTTPBody:[requestStr dataUsingEncoding:NSUTF8StringEncoding]];
            
            
            [NSURLConnection sendAsynchronousRequest:request1
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                       
                                       if(data && !error){
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
                                           
                                           if(object){
                                               NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
                                               if ([[object objectForKey:@"RegionCode"] isKindOfClass:[NSNull class]] ) {
                                                   [defaults setObject:@"" forKey:@"RegionCode"];
                                               }
                                               else [defaults setObject:[object objectForKey:@"RegionCode"] forKey:@"RegionCode"];
                                               
                                               if ([[object objectForKey:@"PhoneNumber"] isKindOfClass:[NSNull class]] ) {
                                                   [defaults setObject:@"" forKey:@"PhoneNumber"];
                                               }
                                               else [defaults setObject:[object objectForKey:@"PhoneNumber"] forKey:@"PhoneNumber"];
                                               
                                               [defaults synchronize];
                                               
                                           }
                                           
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               // Update the UI
                                               [KVNProgress dismiss];
                                               if(![[object objectForKey:@"DataInfo"] isKindOfClass:[NSNull class]]){
                                                   if([[object objectForKey:@"DataInfo"] count]>0){
                                                       if([[NSString stringWithFormat:@"%ld",(long)[[[[object objectForKey:@"DataInfo"] objectAtIndex:0] objectForKey:@"ResultType"] integerValue]] isEqualToString:[NSString stringWithFormat:@"1"]]){
                                                           UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:[[[object objectForKey:@"DataInfo"] objectAtIndex:0] objectForKey:@"Message"] delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                                                           [warningAlert show];
                                                           isEdited = YES;
                                                       }
                                                   }
                                               }
                                               
                                           });
                                       }
                                       else{
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               // Update the UI
                                               [self validateMobileNumber:mobileNumber andIndex:(NoOfTimes+1)];
                                               [KVNProgress dismiss];
                                           });
                                           
                                       }
                                       
                                       
                                       
                                   }];
            
        }
        else{
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
            [alert show];
        }
    }
}

- (IBAction)ComfirmButtonAction:(id)sender {
    
    @try {
        //&& strPhoneNumber.length==10
        [self hideKeyBoard];
        
        if(![[self.txtSecurityCode.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] isEqualToString:@""]){
            if(isEnterpriseUser){
                if([[self.txtEmailSecurityCode.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] isEqualToString:@""]){
                    UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Please Enter Valid EmailId Security Token" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                    [warningAlert show];
                    return;
                }
            }
            [self confirmActionAndIndex:0];
        }
        else{
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Please Enter Valid Mobile Security Token" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}

-(void)confirmActionAndIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        if([[GlobalClass sharedInstance] connected]){
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            
            if(self.isEdit){
                if([prevRegCode isEqualToString:self.txtRegionalCode.text]){
                    [self updatingMobileNumberToServer];
                }
                else{
                    UIAlertView *alert =  [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Changing country will delete all buddies" delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"Ok", nil];
                    alert.tag = 100;
                    [alert show];
                }
                
            }
            else{
                NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
                
                //        LiveDetails *objLiveDetails = (LiveDetails *)[[NSUserDefaults standardUserDefaults] objectForKey:@"LiveDetails"];
                //        NSLog(@"%@",objLiveDetails.authenticationToken);
                
                NSMutableDictionary *liveDetails = [[NSMutableDictionary alloc]init];
                [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"refreshToken"] forKey:@"LiveRefreshToken"];
                [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"accessToken"] forKey:@"LiveAccessToken"];
                
                NSMutableArray *arr = [[NSMutableArray alloc] init];
                NSMutableArray *arr1 = [[NSMutableArray alloc] init];
                
                arr = [[[DBaseInteraction sharedInstance] getAllBuddies] mutableCopy];
                arr1 = [[[DBaseInteraction sharedInstance] getAllGroups] mutableCopy];
                NSMutableArray *arrBuddies = [[NSMutableArray alloc] initWithObjects:nil];
                
                for(int i=0 ;i<[arr count];i++){
                    
                    NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
                    [dict setObject:[[arr objectAtIndex:i] objectForKey:@"Email"] forKey:@"Email"];
                    [dict setObject:[[arr objectAtIndex:i] objectForKey:@"PhoneNumber"] forKey:@"MobileNumber"];
                    [dict setObject:[[arr objectAtIndex:i] objectForKey:@"Name"] forKey:@"Name"];
                    //        [dict setObject:[arr objectAtIndex:0] forKey:@"RegionCode"];
                    if([[[arr objectAtIndex:i] objectForKey:@"BuddyUserId"] length]==0){
                        [dict setObject:@"0" forKey:@"UserID"];
                    }
                    else
                        [dict setObject:[[arr objectAtIndex:i] objectForKey:@"BuddyUserId"] forKey:@"UserID"];
                    
                    if([[[arr objectAtIndex:i] objectForKey:@"BuddyRelationshipId"] length]==0){
                        [dict setObject:@"0" forKey:@"BuddyID"];
                    }
                    else
                        [dict setObject:[[arr objectAtIndex:i] objectForKey:@"BuddyRelationshipId"] forKey:@"BuddyID"];
                    
                    [dict setObject:[[arr objectAtIndex:i] objectForKey:@"State"] forKey:@"State"];
                    [dict setObject:[[arr objectAtIndex:i] objectForKey:@"IsPrimeBuddy"] forKey:@"IsPrimeBuddy"];
                    if([[[arr objectAtIndex:i] objectForKey:@"IsDeleted"] boolValue]){
                        [dict setObject:[NSString stringWithFormat:@"true"] forKey:@"ToRemove"];
                    }
                    else{
                        [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"ToRemove"];
                    }
                    [arrBuddies addObject:dict];
                }
                
                NSMutableArray *arrGroups = [[NSMutableArray alloc] initWithObjects:nil];
                
                for(int i=0 ;i<[arr1 count];i++){
                    
                    NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
                    [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"Email"] forKey:@"Email"];
                    [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"EnrollmentKey"] forKey:@"EnrollmentKey"];
                    [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"EnrollmentType"] forKey:@"EnrollmentType"];
                    [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"EnrollmentValue"] forKey:@"EnrollmentValue"];
                    //        [dict setObject:[[arr1 objectAtIndex:0] objectForKey:@"BuddyUserId"] forKey:@"GeoFence"];
                    [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"GroupId"] forKey:@"GroupID"];
                    [dict setObject:@"" forKey:@"GroupLocation"];
                    [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"Name"] forKey:@"GroupName"];
                    
                    [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"IsActive"];
                    if([[[arr1 objectAtIndex:i] objectForKey:@"IsValidated"] boolValue]){
                        [dict setObject:[NSString stringWithFormat:@"true"] forKey:@"IsValidated"];
                    }
                    else{
                        [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"IsValidated"];
                    }
                    [dict setObject:@"" forKey:@"LiveInfo"];
                    [dict setObject:@"" forKey:@"Members"];
                    
                    [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"PhoneNumber"] forKey:@"MobileNumber"];
                    [dict setObject:@"" forKey:@"Tags"];
                    if([[[arr1 objectAtIndex:i] objectForKey:@"IsDeleted"] boolValue]){
                        [dict setObject:[NSString stringWithFormat:@"true"] forKey:@"ToRemove"];
                    }
                    else{
                        [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"ToRemove"];
                    }
                    [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"Type"] forKey:@"Type"];
                    
                    [arrGroups addObject:dict];
                }
                
                
                NSMutableDictionary *phoneSettings = [[NSMutableDictionary alloc]init];
//                [phoneSettings setValue:@"" forKey:@"ProfileID"];
//                [phoneSettings setValue:@"" forKey:@"DeviceID"];
                [phoneSettings setValue:@"3" forKey:@"PlatForm"];
                [phoneSettings setValue:@"false" forKey:@"CanEmail"];
                [phoneSettings setValue:@"false" forKey:@"CanSMS"];
                
                
                NSMutableDictionary *contentDictionary = [[NSMutableDictionary alloc]init];
                [contentDictionary setValue:@"false" forKey:@"CanArchive"];
                [contentDictionary setValue:@"true" forKey:@"CanMail"];
                [contentDictionary setValue:arrBuddies forKey:@"MyBuddies"];
                [contentDictionary setValue:arrGroups forKey:@"AscGroups"];
                [contentDictionary setValue:self.txtSecurityCode.text forKey:@"SecurityToken"];
                if(isEnterpriseUser){
                    [contentDictionary setValue:self.txtEmailSecurityCode.text forKey:@"EnterpriseSecurityToken"];
                    [contentDictionary setValue:self.txtOrgEmailId.text forKey:@"EnterpriseEmailID"];
                }
                
                
                NSArray *arrPro = [[DBaseInteraction sharedInstance] getAllowancesForProfiles:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                if(arrPro.count>0){
                    if([[[arrPro objectAtIndex:0] valueForKey:@"CanSMS"] boolValue]){
                        [contentDictionary setValue:@"true" forKey:@"CanSMS"];
                    }
                    else{
                        [contentDictionary setValue:@"false" forKey:@"CanSMS"];
                    }
                }
                else{
                    [contentDictionary setValue:@"false" forKey:@"CanSMS"];
                }
                
                [contentDictionary setValue:@"true" forKey:@"LocationConsent"];
                [contentDictionary setValue:@"false" forKey:@"IsTrackingOn"];
                [contentDictionary setValue:[defaults objectForKey:@"email"] forKey:@"Email"];
                [contentDictionary setValue:[defaults objectForKey:@"name"] forKey:@"Name"];
                [contentDictionary setValue:@"false" forKey:@"IsSOSOn"];
                [contentDictionary setValue:liveDetails forKey:@"LiveDetails"];
                [contentDictionary setValue:phoneSettings forKey:@"PhoneSetting"];
                
//                [contentDictionary setValue:@"false" forKey:@"IsValid"];
                [contentDictionary setValue:@"false" forKey:@"CanPost"];
                [contentDictionary setValue:@"I'm in serious trouble. Urgent help needed!." forKey:@"SMSText"];
                
                [contentDictionary setValue:[defaults objectForKey:@"PhoneNumber"] forKey:@"MobileNumber"];
                [contentDictionary setValue:[defaults objectForKey:@"RegionCode"] forKey:@"RegionCode"];
                
                
                
                NSData *data = [NSJSONSerialization dataWithJSONObject:contentDictionary options:NSJSONWritingPrettyPrinted error:nil];
                NSString *jsonStr = [[NSString alloc] initWithData:data
                                                          encoding:NSUTF8StringEncoding];
                
                
                NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:[NSString stringWithFormat:@"%@",kCreateProfileUrl]]
                                                                        cachePolicy: NSURLRequestUseProtocolCachePolicy
                                                                    timeoutInterval: 60.0];
                [request1 setHTTPMethod:@"POST"];
                [request1 setValue:[defaults objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
                [request1 setValue: @"application/json" forHTTPHeaderField: @"Accept"];
                [request1 setValue: @"application/json" forHTTPHeaderField: @"Content-Type"];
                [request1 setHTTPBody:[jsonStr dataUsingEncoding:NSUTF8StringEncoding]];
                
                [defaults synchronize];
                
                [NSURLConnection sendAsynchronousRequest:request1
                                                   queue:[[NSOperationQueue alloc] init]
                                       completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                           if(data && !error){
                                               id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                               NSLog(@"%@",object);
                                               
                                               NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                               NSLog(@"%@",jsonString);
                                               
                                               
                                               if([[object objectForKey:@"DataInfo"] count]>0){
                                                   BOOL result =false;
                                                   for (NSDictionary *dict in [object objectForKey:@"DataInfo"]) {
                                                       if([[dict objectForKey:@"ResultType"] integerValue]==Kcreated){
                                                           [self savePhoneDetails];
                                                           [[GlobalClass sharedInstance] insertProfileDataToDB:[object mutableCopy]];
                                                           result = true;
                                                           [self dismissViewControllerAnimated:YES completion:nil];
                                                           break;
                                                       }
                                                       else if([[dict objectForKey:@"ResultType"] integerValue] == KInvalidToken)
                                                       {
                                                           dispatch_async(dispatch_get_main_queue(), ^{
                                                               // Update the UI
                                                               UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:[dict objectForKey:@"Message"] delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                                                               [warningAlert show];
                                                           });
                                                           result = false;
                                                           break;
                                                       }
                                                       else if([[dict objectForKey:@"ResultType"] integerValue] == KError)
                                                       {
                                                           dispatch_async(dispatch_get_main_queue(), ^{
                                                               // Update the UI
                                                               UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:[dict objectForKey:@"Message"] delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                                                               [warningAlert show];
                                                           });
                                                           result = false;
                                                           break;
                                                       }
                                                   }
                                                   
                                                   if(!result){
                                                       dispatch_async(dispatch_get_main_queue(), ^{
                                                           // Update the UI
                                                           UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Profile not Created" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                                                           [warningAlert show];
                                                       });
                                                   }
                                               }
                                           }
                                           else{
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   // Update the UI
                                                   [self confirmActionAndIndex:(NoOfTimes+1)];
                                               });
                                               
                                           }
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               // Update the UI
                                               [KVNProgress dismiss];
                                           });
                                           
                                       }];
                
            }
            
        }
    }
}

-(void)updatingMobileNumberToServer {
    
    NSMutableArray *arr = [[NSMutableArray alloc] init];
    NSMutableArray *arr1 = [[NSMutableArray alloc] init];
    
    
    
    arr = [[[DBaseInteraction sharedInstance] getAllBuddies] mutableCopy];
    arr1 = [[[DBaseInteraction sharedInstance] getAllGroups] mutableCopy];
    
    NSLog(@"%@",[[[DBaseInteraction sharedInstance] getAllGroups] mutableCopy]) ;
    NSLog(@"%@",arr1);
    
    [[DBaseInteraction sharedInstance] DeleteGroups:arr1];
    [[DBaseInteraction sharedInstance] DeleteBuddies:arr];
    
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    
    
    NSMutableDictionary *liveDetails = [[NSMutableDictionary alloc]init];
    [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"refreshToken"] forKey:@"LiveRefreshToken"];
    [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"accessToken"] forKey:@"LiveAccessToken"];
    [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forKey:@"authenticationToken"];
    //
    [[DBaseInteraction sharedInstance] updatePhoneNumber:[defaults objectForKey:@"PhoneNumber"] forProfileId:[defaults objectForKey:@"ProfileID"]];
    
    NSMutableDictionary *phoneSettings = [[NSMutableDictionary alloc]init];
//    [phoneSettings setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"] forKey:@"ProfileID"];
    //                [phoneSettings setValue:@"" forKey:@"DeviceID"];
    [phoneSettings setValue:[NSString stringWithFormat:@"3"] forKey:@"PlatForm"];
    [phoneSettings setValue:@"false" forKey:@"CanEmail"];
    [phoneSettings setValue:@"false" forKey:@"CanSMS"];
    
    NSMutableArray *arrBuddies = [[NSMutableArray alloc] initWithObjects:nil];
    
    for(int i=0 ;i<[arr count];i++){
        
        NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
        [dict setObject:[[arr objectAtIndex:i] objectForKey:@"Email"] forKey:@"Email"];
        [dict setObject:[[arr objectAtIndex:i] objectForKey:@"PhoneNumber"] forKey:@"MobileNumber"];
        [dict setObject:[[arr objectAtIndex:i] objectForKey:@"Name"] forKey:@"Name"];
        //        [dict setObject:[arr objectAtIndex:0] forKey:@"RegionCode"];
        if([[[arr objectAtIndex:i] objectForKey:@"BuddyUserId"] length]==0){
            [dict setObject:@"0" forKey:@"UserID"];
        }
        else
            [dict setObject:[[arr objectAtIndex:i] objectForKey:@"BuddyUserId"] forKey:@"UserID"];
        
        if([[[arr objectAtIndex:i] objectForKey:@"BuddyRelationshipId"] length]==0){
            [dict setObject:@"0" forKey:@"BuddyID"];
        }
        else
            [dict setObject:[[arr objectAtIndex:i] objectForKey:@"BuddyRelationshipId"] forKey:@"BuddyID"];
        
        [dict setObject:[[arr objectAtIndex:i] objectForKey:@"State"] forKey:@"State"];
        [dict setObject:[[arr objectAtIndex:i] objectForKey:@"IsPrimeBuddy"] forKey:@"IsPrimeBuddy"];
        if([[[arr objectAtIndex:i] objectForKey:@"IsDeleted"] boolValue]){
            [dict setObject:[NSString stringWithFormat:@"true"] forKey:@"ToRemove"];
        }
        else{
            [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"ToRemove"];
        }
        [arrBuddies addObject:dict];
    }
    
    NSMutableArray *arrGroups = [[NSMutableArray alloc] initWithObjects:nil];
    
    for(int i=0 ;i<[arr1 count];i++){
        
        NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
        [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"Email"] forKey:@"Email"];
        [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"EnrollmentKey"] forKey:@"EnrollmentKey"];
        [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"EnrollmentType"] forKey:@"EnrollmentType"];
        [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"EnrollmentValue"] forKey:@"EnrollmentValue"];
        //        [dict setObject:[[arr1 objectAtIndex:0] objectForKey:@"BuddyUserId"] forKey:@"GeoFence"];
        [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"GroupId"] forKey:@"GroupID"];
        [dict setObject:@"" forKey:@"GroupLocation"];
        [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"Name"] forKey:@"GroupName"];
        
        [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"IsActive"];
        if([[[arr1 objectAtIndex:i] objectForKey:@"IsValidated"] boolValue]){
            [dict setObject:[NSString stringWithFormat:@"true"] forKey:@"IsValidated"];
        }
        else{
            [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"IsValidated"];
        }
        [dict setObject:@"" forKey:@"LiveInfo"];
        [dict setObject:@"" forKey:@"Members"];
        
        [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"PhoneNumber"] forKey:@"MobileNumber"];
        [dict setObject:@"" forKey:@"Tags"];
        if([[[arr1 objectAtIndex:i] objectForKey:@"IsDeleted"] boolValue]){
            [dict setObject:[NSString stringWithFormat:@"true"] forKey:@"ToRemove"];
        }
        else{
            [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"ToRemove"];
        }
        [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"Type"] forKey:@"Type"];
        
        [arrGroups addObject:dict];
    }
    
    
    NSMutableDictionary *contentDictionary = [[NSMutableDictionary alloc]init];
    [contentDictionary setValue:@"false" forKey:@"CanArchive"];
    [contentDictionary setValue:@"true" forKey:@"CanMail"];
    [contentDictionary setValue:arrBuddies forKey:@"MyBuddies"];
    [contentDictionary setValue:arrGroups forKey:@"AscGroups"];
    [contentDictionary setValue:self.txtSecurityCode.text forKey:@"SecurityToken"];
    
    if(isEnterpriseUser){
        [contentDictionary setValue:self.txtEmailSecurityCode.text forKey:@"EnterpriseSecurityToken"];
        [contentDictionary setValue:self.txtOrgEmailId.text forKey:@"EnterpriseEmailID"];
    }
    
    NSArray *arrPro = [[DBaseInteraction sharedInstance] getAllowancesForProfiles:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
    if([[[arrPro objectAtIndex:0] valueForKey:@"CanSMS"] boolValue]){
        [contentDictionary setValue:@"true" forKey:@"CanSMS"];
    }
    else{
        [contentDictionary setValue:@"false" forKey:@"CanSMS"];
    }
    
    [contentDictionary setValue:@"true" forKey:@"LocationConsent"];
    
    [contentDictionary setValue:@"false" forKey:@"IsTrackingOn"];
    [contentDictionary setValue:[defaults objectForKey:@"name"] forKey:@"Name"];
    
    if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
        [contentDictionary setValue:@"false" forKey:@"IsSOSOn"];
    }
    else{
        [contentDictionary setValue:@"true" forKey:@"IsSOSOn"];
    }
    [contentDictionary setValue:liveDetails forKey:@"LiveDetails"];
    [contentDictionary setValue:phoneSettings forKey:@"PhoneSetting"];
    
    [contentDictionary setValue:@"true" forKey:@"IsValid"];
    [contentDictionary setValue:@"false" forKey:@"CanPost"];
    [contentDictionary setValue:@"I'm in serious trouble. Urgent help needed!." forKey:@"SMSText"];
    
    [contentDictionary setValue:[defaults objectForKey:@"email"] forKey:@"Email"];
    [contentDictionary setValue:[defaults objectForKey:@"ProfileID"] forKey:@"ProfileID"];
    [contentDictionary setValue:[defaults objectForKey:@"UserID"] forKey:@"UserID"];
    
    
    [contentDictionary setValue:[defaults objectForKey:@"PhoneNumber"] forKey:@"MobileNumber"];
    [contentDictionary setValue:[defaults objectForKey:@"RegionCode"] forKey:@"RegionCode"];
    
    
    
    NSData *data = [NSJSONSerialization dataWithJSONObject:contentDictionary options:NSJSONWritingPrettyPrinted error:nil];
    NSString *jsonStr = [[NSString alloc] initWithData:data
                                              encoding:NSUTF8StringEncoding];
    
    NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:[NSString stringWithFormat:@"%@",kupdatePhoneProfile]]
                                                            cachePolicy: NSURLRequestUseProtocolCachePolicy
                                                        timeoutInterval: 60.0];
    [request1 setHTTPMethod:@"POST"];
    [request1 setValue:[defaults objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
    [request1 setValue: @"application/json" forHTTPHeaderField:@"Accept"];
    [request1 setValue: @"application/json" forHTTPHeaderField:@"Content-Type"];
    [request1 setHTTPBody:[jsonStr dataUsingEncoding:NSUTF8StringEncoding]];
    
    [defaults synchronize];
    
    AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    appdele.settingChanged = YES;
    
    [NSURLConnection sendAsynchronousRequest:request1
                                       queue:[[NSOperationQueue alloc] init]
                           completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                               if(data){
                                   id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                   NSLog(@"%@",object);
                                   prevRegCode = self.txtRegionalCode.text;
                                   [self savePhoneDetails];
                                   [[GlobalClass sharedInstance] insertProfileDataToDB:[object mutableCopy]];
                               }
                               dispatch_async(dispatch_get_main_queue(), ^{
                                   // Update the UI
                                   [self dismissViewControllerAnimated:YES completion:nil];
                                   [KVNProgress dismiss];
                               });

                           }];
    
    

}

-(void)savePhoneDetails{
//    [[NSUserDefaults standardUserDefaults] setObject:self.txtRegionalCode.text forKey:@"RegionCode"];
//    [[NSUserDefaults standardUserDefaults] setObject:self.txtNumberField.text forKey:@"PhoneNumber"];
    isEdited = NO;
    NBPhoneNumberUtil *phoneUtil = [[NBPhoneNumberUtil alloc] init];
    NSNumber *num1 = @([self.txtRegionalCode.text integerValue]);
    NSLog(@"%@",[phoneUtil getRegionCodeForCountryCode:num1]) ;
    [[NSUserDefaults standardUserDefaults] setObject:[phoneUtil getRegionCodeForCountryCode:num1] forKey:@"LocaleCode"];
}

# pragma mark UIPickerViewDelegateAndDataSources

- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
{
    return 1;
}

// The number of rows of data
- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component
{
        return arrCountryList.count;
}

// The data to return for the row and component (column) that's being passed in
- (NSString*)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    
        return [NSString stringWithFormat:@"%@ %@",[[arrCountryList objectAtIndex:row] objectForKey:@"IsdCode"],[[arrCountryList objectAtIndex:row] objectForKey:@"Name"]];
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component{
        prevString = self.txtRegionalCode.text;
        selIndex = row;
        self.txtRegionalCode.text = [[arrCountryList objectAtIndex:row] objectForKey:@"IsdCode"];
}

#pragma mark UiAlertView Delegate Methods
- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex{
    if(alertView.tag==100){
        @try {
            if(buttonIndex == 1){
                
                NSArray *arr = [[DBaseInteraction sharedInstance] getAllBuddies];
                for (NSDictionary *dict in arr) {
                     [[DBaseInteraction sharedInstance] DeleteBuddyEdit:[dict objectForKey:@"PhoneNumber"]];
                }
                
                [self updatingMobileNumberToServer];
                [[NSUserDefaults standardUserDefaults] removeObjectForKey:@"DefaultCaller"];
            }
            else{
                [[NSUserDefaults standardUserDefaults] setObject:self.Phonetxt forKey:@"PhoneNumber"];
                self.txtRegionalCode.text = prevRegCode;
                self.txtNumberField.text = [self.Phonetxt stringByReplacingOccurrencesOfString:self.txtRegionalCode.text withString:@""];
                NSArray *filteredarray = [arrCountryList filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(IsdCode == %@)", [[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"]]];
                if(filteredarray.count>0){
                    maxPhoneDigit = [[[filteredarray objectAtIndex:0] objectForKey:@"MaxPhoneDigits"] integerValue];
                }
                [[NSUserDefaults standardUserDefaults] setObject:self.txtRegionalCode.text forKey:@"RegionCode"];
                [KVNProgress dismiss];
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


#pragma mark TextField delegate methods

-(void)hideKeyBoard{
    if(isKeyboardShown){
        for(UIView *t in self.scrlView.subviews){
            if([t isKindOfClass:[UITextField class]]){
                [t resignFirstResponder];
                [self.scrlView setContentOffset:CGPointMake(0, 0) animated:YES];
            }
        }
        for(UIView *t in self.viewConfirm.subviews){
            if([t isKindOfClass:[UITextField class]]){
                [t resignFirstResponder];
                [self.scrlView setContentOffset:CGPointMake(0, 0) animated:YES];
            }
        }
        for(UIView *t in self.viewValidate.subviews){
            if([t isKindOfClass:[UITextField class]]){
                [t resignFirstResponder];
                [self.scrlView setContentOffset:CGPointMake(0, 0) animated:YES];
            }
        }
    }
}

-(void)keyboardDidShow:(NSNotification *)notify{
    isKeyboardShown = YES;
}
-(void)keyboardDidHide:(NSNotification *)notify{
    isKeyboardShown = NO;
}


- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString *)string
{
    if(textField.tag == 1){
        NSString *currentString = [textField.text stringByReplacingCharactersInRange:range withString:string];
        NSInteger length = [currentString length];
        if (length > maxPhoneDigit) {
            return NO;
        }
        if([string isEqualToString:@""]){
            return YES;
        }
    }
    return YES;
}

//- (void)textFieldDidBeginEditing:(UITextField *)textField{
//    if(textField.tag == 1){
//        [self.scrlView setContentOffset:CGPointMake(0, 100) animated:YES];
//    }
//    else if(textField.tag == 2){
//        [self.scrlView setContentOffset:CGPointMake(0, 180) animated:YES];
//    }
//}
//
//- (void)textFieldDidEndEditing:(UITextField *)textField{
//    [self.scrlView setContentOffset:CGPointMake(0, 0) animated:YES];
//}
//
-(BOOL)textFieldShouldReturn:(UITextField *)textField{
    [textField resignFirstResponder];
    return YES;
}



@end

