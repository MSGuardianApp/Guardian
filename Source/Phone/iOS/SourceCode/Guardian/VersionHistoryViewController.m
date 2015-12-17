//
//  VersionHistoryViewController.m
//  Guardian
//
//  Created by PTG on 10/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "VersionHistoryViewController.h"

@interface VersionHistoryViewController ()
@property (weak, nonatomic) IBOutlet UILabel *lblversion3;
@property (weak, nonatomic) IBOutlet UILabel *lblVersion3Desc;
@property (weak, nonatomic) IBOutlet UILabel *lblVersion2Desc;
@property (weak, nonatomic) IBOutlet UILabel *lblVersion2;
@property (weak, nonatomic) IBOutlet UILabel *lblVersion1;
@property (weak, nonatomic) IBOutlet UILabel *lblVersion1Desc;
@property (weak, nonatomic) IBOutlet UILabel *lblVersion0;
@property (weak, nonatomic) IBOutlet UILabel *lblVersion0Desc;

@end

@implementation VersionHistoryViewController

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
    [self setfontForlabels];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}
-(void)setfontForlabels {
    self.lblversion3.font = [UIFont fontWithName:@"SegoeUI" size:self.lblversion3.font.pointSize];
    self.lblVersion3Desc.font = [UIFont fontWithName:@"SegoeUI" size:self.lblVersion3Desc.font.pointSize];
    self.lblVersion2Desc.font = [UIFont fontWithName:@"SegoeUI" size:self.lblVersion2Desc.font.pointSize];
    self.lblVersion2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblVersion2.font.pointSize];
    self.lblVersion1.font = [UIFont fontWithName:@"SegoeUI" size:self.lblVersion1.font.pointSize];
    self.lblVersion1Desc.font = [UIFont fontWithName:@"SegoeUI" size:self.lblVersion1Desc.font.pointSize];
    self.lblVersion0.font = [UIFont fontWithName:@"SegoeUI" size:self.lblVersion0.font.pointSize];
    self.lblVersion0Desc.font = [UIFont fontWithName:@"SegoeUI" size:self.lblVersion0Desc.font.pointSize];
}
@end
