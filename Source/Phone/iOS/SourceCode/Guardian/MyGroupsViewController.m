//
//  MyGroupsViewController.m
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "MyGroupsViewController.h"
#import "MyBuddiesCustomCell.h"
#import "RegisterViewController.h"


@interface MyGroupsViewController ()
@property (nonatomic , weak) IBOutlet UIView *viewMicrosoftTile;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect1;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect2;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect3;
@property (nonatomic , retain) IBOutlet UITableView *tblMyGroups;

@property (nonatomic , retain) IBOutlet UIButton *btnPrivacy;

-(IBAction)btnPrivacyPoilcyClicked:(id)sender;
@end

@implementation MyGroupsViewController

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
    self.arrGroups = [[NSMutableArray alloc] init];
    
    NSLog(@"%@",[NSUserDefaults standardUserDefaults].dictionaryRepresentation);
    
    [self setfontForlabels];
    
    // Do any additional setup after loading the view from its nib.
}

-(void)viewWillAppear:(BOOL)animated{
    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
        
        [self.viewMicrosoftTile setFrame:CGRectMake(0, 10, 320,[UIScreen mainScreen].applicationFrame.size.height-65)];
        [self.view addSubview:self.viewMicrosoftTile];
        //        [self.scrlView removeFromSuperview];
    }
    else{
        
        [self getGroupsfromDB];
        //      [self updateMaingroupTable];
    }
}


-(void)getGroupsfromDB{
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    if([defaults boolForKey:@"ProfileInserted"]){
        @try {
            self.arrGroups = [[[DBaseInteraction sharedInstance] getGroups] mutableCopy];
        }
        @catch (NSException *exception) {
            [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
            NSLog(@"%@",exception);
        }
        @finally {
            
        }
        
        [self.tblMyGroups reloadData];
    }
    else{
        [self.viewMicrosoftTile setFrame:CGRectMake(0, 10, 320,[UIScreen mainScreen].applicationFrame.size.height-65)];
        [self.view addSubview:self.viewMicrosoftTile];
    }
    
}

-(void)setfontForlabels {
    self.lblLiveConnect1.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect1.font.pointSize];
    self.lblLiveConnect2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect2.font.pointSize];
    self.lblLiveConnect3.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect3.font.pointSize];
    self.btnPrivacy.titleLabel.font = [UIFont fontWithName:@"SegoeUI" size:self.btnPrivacy.titleLabel.font.pointSize];
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


-(UIViewController*) getRootViewController {
    return [UIApplication sharedApplication].keyWindow.rootViewController;
}

#pragma mark UITableViewCells -UITableViewDelegate

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section;
{
    return self.arrGroups.count;
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
    
    cell.lblName.text = [[self.arrGroups objectAtIndex:indexPath.row] objectForKey:@"Name"];
    
    cell.lblPhoneNumber.text = [[self.arrGroups objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"];;
    
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
    
    @try {
        NSIndexPath * indexPath = [self.tblMyGroups indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblMyGroups]];
        if ( indexPath == nil )
            return;
        
        UIDevice *device = [UIDevice currentDevice];
        
        NSString *cellNameStr = [NSString stringWithFormat:@"%@",[[self.arrGroups objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]];
        
        if ([[device model] isEqualToString:@"iPhone"] ) {
            
            NSString *phoneNumber = [@"telprompt://" stringByAppendingString:cellNameStr];
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:phoneNumber]];
            
        } else {
            
            UIAlertView *warning =[[UIAlertView alloc] initWithTitle:@"Call" message:@"Your device doesn't support this feature." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            
            [warning show];
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
    
}

- (void) accessorySMSButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    
    @try {
        NSIndexPath * indexPath = [self.tblMyGroups indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblMyGroups]];
        if ( indexPath == nil )
            return;
        
        if(![MFMessageComposeViewController canSendText]) {
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Your device doesn't support SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
            return;
        }
        
        NSMutableArray *arr= [[NSMutableArray alloc] init];
        NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
        if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
            arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
        }
        GeoTag *obj = (GeoTag *)[arr lastObject];
		if(![MFMessageComposeViewController canSendText]) {
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Your device doesn't support SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
            return;
        }
		
        MFMessageComposeViewController *messageController = [[MFMessageComposeViewController alloc] init];
        messageController.messageComposeDelegate = self;
        [messageController setRecipients:[NSArray arrayWithObject:[[self.arrGroups objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]]];
        [messageController setBody:[NSString stringWithFormat:@"I'm in serious trouble. Urgent help needed!. I'm @ Lati:%@ Longi:%@",obj.Lati,obj.Longi]];
        
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
        NSIndexPath * indexPath = [self.tblMyGroups indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblMyGroups]];
        if ( indexPath == nil )
            return;
        
//        if([[self.arrGroups objectAtIndex:indexPath.row] objectForKey:@"Email"] && [[[self.arrGroups objectAtIndex:indexPath.row] objectForKey:@"Email"] length]>0){
            NSString *emailTitle = @"Guardian";
            // Email Content
            NSMutableArray *arr= [[NSMutableArray alloc] init];
            NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
            if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
                arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
            }
            GeoTag *obj = (GeoTag *)[arr lastObject];
            NSString *messageBody = [NSString stringWithFormat:@"I'm in serious trouble. Urgent help needed!. I'm @ Lati:%@ Longi:%@",obj.Lati,obj.Longi];
            // To address
            NSArray *toRecipents = [NSArray arrayWithObject:[[self.arrGroups objectAtIndex:indexPath.row] objectForKey:@"Email"]];
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
