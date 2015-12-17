//
//  LoggerViewController.m
//  Guardian
//
//  Created by PTG on 10/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "LoggerViewController.h"

@interface LoggerViewController ()
@property (weak, nonatomic) IBOutlet UILabel *lblTop;
- (IBAction)sendReport:(id)sender;
@end

@implementation LoggerViewController

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
    self.lblTop.font = [UIFont fontWithName:@"SegoeUI" size:self.lblTop.font.pointSize];
    
}

- (IBAction)sendReport:(id)sender{
    @try {
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory,NSUserDomainMask, YES);
        NSString *documentsDirectory = [paths objectAtIndex:0];
        NSString *txtFilePath = [documentsDirectory stringByAppendingPathComponent:@"crashLogfile.txt"];
        if(txtFilePath.length>0){
            NSData *noteData = [NSData dataWithContentsOfFile:txtFilePath];
            
            
            // Email Subject
            NSString *emailTitle = @"Guardian";
            // Email Content
            NSString *messageBody = @"Guardian is the ultimate security app in your hand. Being easy to configure and the power of being tracked by buddies and security groups simultaneously, ensures multi-fold security for you. Download Guandian App : http://www.windowsphone.com/en-in/store/app/guardian/178406e1-0363-43ee-8be0-e2945fa18d6b";
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
                [mc addAttachmentData:noteData mimeType:@"text/plain" fileName:@"crashLogfile.txt"];
                [mc setToRecipients:toRecipents];
                
                // Present mail view controller on screen
                [self presentViewController:mc animated:YES completion:NULL];
            }
           
        }
        else{
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"No reports available." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
            [alert show];
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
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
            {
                NSLog(@"Mail sent");
                
                NSArray *paths = NSSearchPathForDirectoriesInDomains
                (NSDocumentDirectory, NSUserDomainMask, YES);
                NSString *documentsDirectory = [paths objectAtIndex:0];
                
                //make a file name to write the data to using the documents directory:
                NSString *fileName = [NSString stringWithFormat:@"%@/crashLogfile.txt",
                                      documentsDirectory];
                //create content - four lines of text
                // NSString *content = @"One\nTwo\nThree\nFour\nFive";
                
                NSString *content = [NSString stringWithFormat:@""];
                //save content to the documents directory
                [content writeToFile:fileName
                          atomically:NO
                            encoding:NSStringEncodingConversionAllowLossy
                               error:nil];
                
                break;
            }
            
        case MFMailComposeResultFailed:
            NSLog(@"Mail sent failure: %@", [error localizedDescription]);
            break;
        default:
            break;
    }
    
    // Close the Mail Interface
    [self dismissViewControllerAnimated:YES completion:NULL];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
