//
//  MyBuddiesViewController.m
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "MyBuddiesViewController.h"
#import "MyBuddiesCustomCell.h"

@interface MyBuddiesViewController ()
@property (nonatomic ,weak) IBOutlet UITableView *tblMyBuddies;
-(IBAction)policeBtnCicked:(UIButton *)sender;
-(IBAction)ambulanceBtnCicked:(UIButton *)sender;
-(IBAction)fireBtnCicked:(UIButton *)sender;
@end

@implementation MyBuddiesViewController

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
    self.arrBuddies = [[NSMutableArray alloc] init];
   
    [self getBuddiesfromDB];
    // Do any additional setup after loading the view from its nib.
}

-(void)getBuddiesfromDB{
    @try {
        self.arrBuddies = [[[DBaseInteraction sharedInstance] getBuddyData] mutableCopy];
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}


#pragma mark IBAction Methods

-(IBAction)policeBtnCicked:(UIButton *)sender{
    [self doCallToNumber:@"100"];
}
-(IBAction)ambulanceBtnCicked:(UIButton *)sender{
    [self doCallToNumber:@"108"];
}
-(IBAction)fireBtnCicked:(UIButton *)sender{
    [self doCallToNumber:@"101"];
}



-(void)doCallToNumber:(NSString *)phoneNum{
    UIDevice *device = [UIDevice currentDevice];
    
    NSString *cellNameStr = [NSString stringWithFormat:@"%@",phoneNum];
    
    if ([[device model] isEqualToString:@"iPhone"] ) {
        
        NSString *phoneNumber = [@"telprompt://" stringByAppendingString:cellNameStr];
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:phoneNumber]];
        
    } else {
        
        UIAlertView *warning =[[UIAlertView alloc] initWithTitle:@"Call" message:@"Your device doesn't support this feature." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
        
        [warning show];
    }
}
#pragma mark UITableViewCells -UITableViewDelegate

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section;
{
     return self.arrBuddies.count;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath{
    
        static NSString *cellIdentifier=@"Cell";
        MyBuddiesCustomCell *cell=(MyBuddiesCustomCell *)[tableView dequeueReusableCellWithIdentifier:cellIdentifier];
        if (cell==nil) {
            NSArray *array=[[NSBundle mainBundle]loadNibNamed:@"MyBuddiesCustomCell" owner:self options:nil];
            cell=(MyBuddiesCustomCell *)[array objectAtIndex:0];
        }
    
        cell.selectionStyle = UITableViewCellSelectionStyleNone;
    
        cell.lblName.font = [UIFont fontWithName:@"SegoeUI" size:cell.lblName.font.pointSize];
        cell.lblPhoneNumber.font = [UIFont fontWithName:@"SegoeUI" size:cell.lblPhoneNumber.font.pointSize];
    
        cell.lblName.text = [[self.arrBuddies objectAtIndex:indexPath.row] objectForKey:@"Name"];
        
        cell.lblPhoneNumber.text = [[self.arrBuddies objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"];;
        
        [cell.btnCall addTarget:self action:@selector(accessoryCallButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
        cell.btnCall.tag = indexPath.row;
    
        [cell.btnSMS addTarget:self action:@selector(accessorySMSButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
        cell.btnSMS.tag = indexPath.row;
    
        [cell.btnMail addTarget:self action:@selector(accessoryMailButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
        cell.btnMail.tag = indexPath.row;
    
        cell.viewBG.layer.borderWidth =1.0f;
        cell.viewBG.layer.borderColor = [UIColor whiteColor].CGColor;
    
        cell.btnCall.layer.borderWidth =1.0f;
        cell.btnCall.layer.borderColor = [UIColor whiteColor].CGColor;
        
        cell.btnSMS.layer.borderWidth =1.0f;
        cell.btnSMS.layer.borderColor = [UIColor whiteColor].CGColor;
        
        cell.btnMail.layer.borderWidth =1.0f;
        cell.btnMail.layer.borderColor = [UIColor whiteColor].CGColor;
    
        return cell;
        
}


- (void) accessoryCallButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    NSIndexPath * indexPath = [self.tblMyBuddies indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblMyBuddies]];
    if ( indexPath == nil )
        return;
    
    [self doCallToNumber:[NSString stringWithFormat:@"%@",[[self.arrBuddies objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]]];
    
//    NSString *aPhoneNo = [@"telprompt://" stringByAppendingString:[[self.arrBuddies objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]] ;
//    NSURL *url= [NSURL URLWithString:aPhoneNo];
//    NSString *osVersion = [[UIDevice currentDevice] systemVersion];
//    
//    if ([osVersion floatValue] >= 3.1) {
//        UIWebView *webview = [[UIWebView alloc] initWithFrame:[UIScreen mainScreen].applicationFrame];
//        [webview loadRequest:[NSURLRequest requestWithURL:url]];
//        webview.hidden = YES;s
//        // Assume we are in a view controller and have access to self.view
//        [self.view addSubview:webview];
//    } else {
//        // On 3.0 and below, dial as usual
//        [[UIApplication sharedApplication] openURL: url];
//    }
    
//    UIDevice *device = [UIDevice currentDevice];
//    
//    NSString *cellNameStr = [NSString stringWithFormat:@"%@",[[self.arrBuddies objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]];
//    
//    if ([[device model] isEqualToString:@"iPhone"] ) {
//        
//        NSString *phoneNumber = [@"telprompt://" stringByAppendingString:cellNameStr];
//        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:phoneNumber]];
//        
//    } else {
//        
//        UIAlertView *warning =[[UIAlertView alloc] initWithTitle:@"Call" message:@"Your device doesn't support this feature." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
//        
//        [warning show];
//    }
    
}

- (void) accessorySMSButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    @try {
        NSIndexPath * indexPath = [self.tblMyBuddies indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblMyBuddies]];
        if ( indexPath == nil )
            return;
        
        if(![MFMessageComposeViewController canSendText]) {
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Your device doesn't support SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
            return;
        }
        
        [self msgPreparationAndForIndex:indexPath.row andISMail:NO];
        
//        if(bodyText.length > 0){
//           [self sendSMSByMsg:bodyText andTo:[[self.arrBuddies objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]];
//        }
        
        
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
        NSIndexPath * indexPath = [self.tblMyBuddies indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblMyBuddies]];
        if ( indexPath == nil )
            return;
        
//        if([[self.arrBuddies objectAtIndex:indexPath.row] objectForKey:@"Email"] && [[[self.arrBuddies objectAtIndex:indexPath.row] objectForKey:@"Email"] length]>0){
        
        [self msgPreparationAndForIndex:indexPath.row andISMail:YES];
        
//            if(bodyText.length > 0){
//                [self sendMailByMsg:bodyText andTo:[[self.arrBuddies objectAtIndex:indexPath.row] objectForKey:@"Email"]];
//            }
//            
            
//        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}


- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    return 57;
}


-(void)sendMailByMsg:(NSString *)mailBody andTo:(NSString *)toAddress{
    NSString *emailTitle = @"Guardian";
    // Email Content
    NSString *messageBody = mailBody;
    // To address
    NSArray *toRecipents = [NSArray arrayWithObject:toAddress];
    
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

-(void)sendSMSByMsg:(NSString *)smsBody andTo:(NSString *)toAddress{
	if(![MFMessageComposeViewController canSendText]) {
        UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Your device doesn't support SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
        [warningAlert show];
        return;
    }
    MFMessageComposeViewController *messageController = [[MFMessageComposeViewController alloc] init];
    messageController.messageComposeDelegate = self;
    [messageController setRecipients:[NSArray arrayWithObject:toAddress]];
    [messageController setBody:smsBody];
    
    // Present message view controller on screen
    [self presentViewController:messageController animated:YES completion:nil];
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

-(NSString *)msgPreparationAndForIndex:(NSInteger)indexpath andISMail:(BOOL)isMail{
    
    @try {
        if(self.arrBuddies.count > 0){
            msg =@"";
            if([[GlobalClass sharedInstance] connected]){
                if([[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
                    NSArray *arrProf = [[DBaseInteraction sharedInstance] getProfile];
                    
                    msg = [NSString stringWithFormat:@"%@ %@ needs urgent help at",[[NSUserDefaults standardUserDefaults] objectForKey:@"first_name"],[[arrProf objectAtIndex:0] objectForKey:@"MobileNumber"]];
                    
                    if([[GlobalClass sharedInstance] connected]){
                        
                        NSString *strSampleUrl = [NSString stringWithFormat:@"https://guardianportal.cloudapp.net/default.aspx?pr=%@&s=%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"]];
                        
                        
                        
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
                                                           msg = [NSString stringWithFormat:@"%@  %@",msg,jsonString];
                                                           if(isMail){
                                                               [self sendMailByMsg:msg andTo:[[self.arrBuddies objectAtIndex:indexpath] objectForKey:@"Email"]];
                                                           }
                                                           else{
                                                               [self sendSMSByMsg:msg andTo:[[self.arrBuddies objectAtIndex:indexpath] objectForKey:@"PhoneNumber"]];
                                                           }
                                                           
                                                           
                                                       });
                                                   }
                                                   
                                               }];
                        return msg;
                    }
                    
                    else{
                    }
                    
                    
                }
                else{
                    msg = [self offlineUnRegisteredSMStext];
                }
            }
            else{
                if([[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
                    msg = [self offlineRegisteredSMStext];
                }
                else{
                    msg = [self offlineUnRegisteredSMStext];
                }
                
            }
            
            
        }
        return msg;
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
        GeoTag *obj = (GeoTag *)[arr lastObject];
        strMesg = [NSString stringWithFormat:@"%@ I'm at %@ /default.aspx?V=2&track=%@%@&lat=%@&long=%@&tick=%@",kMessageTemplateText,kGuardianPortalUrl,[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"],obj.Lati,obj.Longi,[[GlobalClass sharedInstance] dateToTicks:[NSDate date]]];
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
        strMesg = [NSString stringWithFormat:@"%@ I'm at %@/default.aspx?V=2&lat=%@&long=%@",kMessageTemplateText,kGuardianPortalUrl,obj.Lati,obj.Longi];
    }
    
    return strMesg;
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
