//
//  HowItWorksViewController.m
//  Guardian
//
//  Created by PTG on 10/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "HowItWorksViewController.h"

@interface HowItWorksViewController ()
@property (weak, nonatomic) IBOutlet UITextView *textView;

@end

@implementation HowItWorksViewController

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
    self.textView.font = [UIFont fontWithName:@"SegoeUI" size:self.textView.font.pointSize];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
