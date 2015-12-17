//
//  AboutHostViewController.m
//  Guardian
//
//  Created by PTG on 10/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "AboutHostViewController.h"
#import "AboutViewController.h"
#import "HowItWorksViewController.h"
#import "FeaturesViewController.h"
#import "VersionHistoryViewController.h"
#import "LoggerViewController.h"
@interface AboutHostViewController ()
@property (nonatomic) NSUInteger numberOfTabs;
- (IBAction)sendLinkTapped:(id)sender;
- (IBAction)messageLinkTapped:(id)sender;
- (IBAction)shareLinkTapped:(id)sender;
- (IBAction)rateAppTapped:(id)sender;
- (IBAction)homeButtonTapped:(id)sender;

@end

@implementation AboutHostViewController

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
    arrContent = [[NSMutableArray alloc] initWithObjects:@"About",@"HowItWorks",@"Features",@"VersionHistory",@"Logger", nil];
    
    self.dataSource = self;
    self.delegate = self;
    self.view.backgroundColor = [UIColor clearColor];

}
- (void)viewDidAppear:(BOOL)animated {
    
    [super viewDidAppear:animated];
    
    [self performSelector:@selector(loadContent) withObject:nil afterDelay:0];
    
}
#pragma mark UIActionSheet Delegate Methods
#pragma mark

-(void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex {
    if(buttonIndex == 0){
        if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
        }
        else{
            
            
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:[NSString stringWithFormat:@"%@",kUnregisterUrl]]
                                                                    cachePolicy: NSURLRequestUseProtocolCachePolicy
                                                                timeoutInterval: 60.0];
            [request1 setHTTPMethod:@"GET"];
            //            LiveDetails *objLiveDetails = (LiveDetails *)[[NSUserDefaults standardUserDefaults] objectForKey:@"LiveDetails"];
            //            NSLog(@"%@",objLiveDetails.authenticationToken);
            
            [request1 setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
            [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
            
            
            [NSURLConnection sendAsynchronousRequest:request1
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                       id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                       NSLog(@"%@",object);
                                       
                                       dispatch_async(dispatch_get_main_queue(), ^{
                                       });
                                   }];
            
        }
        
        
    }
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
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
    self.numberOfTabs = 5;
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
        vc = [[AboutViewController alloc] init];
    }
    else if(index == 1){
        vc = [[HowItWorksViewController alloc] init];
    }else if(index == 2){
        vc = [[FeaturesViewController alloc] init];
    }else if(index == 3){
        vc = [[VersionHistoryViewController alloc] init];
    }else if(index == 4){
        vc = [[LoggerViewController alloc] init];
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

- (IBAction)sendLinkTapped:(id)sender {
    // Email Subject
    NSString *emailTitle = @"Guardian App download";
    // Email Content
    NSString *messageBody = @"Guardian is the ultimate security app in your hand. Being easy to configure and the power of being tracked by buddies and security groups simultaneously, ensures multi-fold security for you. Download Guandian App : http://itunes.apple.com/in/app/GuardianApp/id979153515?mt=8";
    // To address
    NSArray *toRecipents = [NSArray arrayWithObject:@""];
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
- (IBAction)messageLinkTapped:(id)sender {
    if(![MFMessageComposeViewController canSendText]) {
        UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Your device doesn't support SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
        [warningAlert show];
        return;
    }
    
    NSArray *recipents = @[@""];
    NSString *message = [NSString stringWithFormat:@"Download Guandian App at : http://itunes.apple.com/in/app/GuardianApp/id979153515?mt=8 "];
    
    MFMessageComposeViewController *messageController = [[MFMessageComposeViewController alloc] init];
    messageController.messageComposeDelegate = self;
    [messageController setRecipients:recipents];
    [messageController setBody:message];
    
    // Present message view controller on screen
    [self presentViewController:messageController animated:YES completion:nil];
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

- (IBAction)shareLinkTapped:(id)sender {
    NSString *text = @"Guardian is the ultimate security app in your hand. Being easy to configure and the power of being tracked by buddies and security groups simultaneously, ensures multi-fold security for you. Download Guandian App : http://itunes.apple.com/in/app/GuardianApp/id979153515?mt=8";
    UIActivityViewController *controller =[[UIActivityViewController alloc] initWithActivityItems:@[text] applicationActivities:nil];
    
    [self presentViewController:controller animated:YES completion:nil];
}
- (IBAction)rateAppTapped:(id)sender {
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"itms://itunes.apple.com/in/app/guardianapp/id979153515?mt=8"]];
}

- (IBAction)homeButtonTapped:(id)sender {
    [self.navigationController popViewControllerAnimated:YES];
}
@end
