//
//  AboutViewController.m
//  Guardian
//
//  Created by PTG on 10/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "AboutViewController.h"

@interface AboutViewController ()
@property (weak, nonatomic) IBOutlet UIScrollView *myScrView;
- (IBAction)mailButtonTapped:(id)sender;
@property (weak, nonatomic) IBOutlet UILabel *lblTop;
    
@property (weak, nonatomic) IBOutlet UIButton *lblMail;
@property (weak, nonatomic) IBOutlet UILabel *lblMicrosoft;
@property (weak, nonatomic) IBOutlet UILabel *lblPoweredBy;
@property (weak, nonatomic) IBOutlet UILabel *lblFeedback;
@property (weak, nonatomic) IBOutlet UILabel *lblVersion;
@property (weak, nonatomic) IBOutlet UILabel *lblRelease;
@property (weak, nonatomic) IBOutlet UILabel *lblSecurity;
@end

@implementation AboutViewController

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
    [self.myScrView setContentSize:CGSizeMake(320,410)];
    [self setfontForlabels];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}
-(void)setfontForlabels {
    self.lblTop.font = [UIFont fontWithName:@"SegoeUI" size:self.lblTop.font.pointSize];
    self.lblMicrosoft.font = [UIFont fontWithName:@"SegoeUI" size:self.lblMicrosoft.font.pointSize];
    self.lblPoweredBy.font = [UIFont fontWithName:@"SegoeUI" size:self.lblPoweredBy.font.pointSize];
    self.lblFeedback.font = [UIFont fontWithName:@"SegoeUI" size:self.lblFeedback.font.pointSize];
    self.lblVersion.font = [UIFont fontWithName:@"SegoeUI" size:self.lblVersion.font.pointSize];
    self.lblRelease.font = [UIFont fontWithName:@"SegoeUI" size:self.lblRelease.font.pointSize];
    self.lblSecurity.font = [UIFont fontWithName:@"SegoeUI" size:self.lblSecurity.font.pointSize];
    
}
- (IBAction)mailButtonTapped:(id)sender {
    // Email Subject
    NSString *emailTitle = @"";
    // Email Content
    NSString *messageBody = @"";
    // To address
    NSArray *toRecipents = [NSArray arrayWithObject:@"guardianapp@outlook.com"];
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
@end
