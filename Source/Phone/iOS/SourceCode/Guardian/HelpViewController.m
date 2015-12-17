//
//  HelpViewController.m
//  Guardian
//
//  Created by PTG on 16/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "HelpViewController.h"

@interface HelpViewController ()
@property (weak, nonatomic) IBOutlet UIScrollView *scrView;
@property (strong, nonatomic) UIImageView *imgView;

- (IBAction)homeButtonTapped:(id)sender;
@end

@implementation HelpViewController

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
    indexOfArticle = 0;
    // Do any additional setup after loading the view from its nib.
    [self imageLoad];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}
-(void)imageLoad{
    images = [[NSMutableArray alloc] initWithObjects:[UIImage imageNamed:@"h1.png"],[UIImage imageNamed:@"h3.png"],[UIImage imageNamed:@"h4.png"],[UIImage imageNamed:@"h5.png"],[UIImage imageNamed:@"h6.png"],[UIImage imageNamed:@"h7.png"],[UIImage imageNamed:@"h8.png"],[UIImage imageNamed:@"h9.png"],[UIImage imageNamed:@"h10.png"],[UIImage imageNamed:@"h11.png"],[UIImage imageNamed:@"h12.png"],[UIImage imageNamed:@"h13.png"],[UIImage imageNamed:@"h14.png"],[UIImage imageNamed:@"h15.png"],[UIImage imageNamed:@"h16.png"],[UIImage imageNamed:@"h17.png"],[UIImage imageNamed:@"h18.png"],[UIImage imageNamed:@"h19.png"],[UIImage imageNamed:@"h20.png"],[UIImage imageNamed:@"h21.png"],[UIImage imageNamed:@"h22.png"],[UIImage imageNamed:@"h23.png"],[UIImage imageNamed:@"h24.png"],[UIImage imageNamed:@"h25.png"],[UIImage imageNamed:@"h26.png"], nil];
    
    for (int i = 0; i < [images count]; i++) {
        self.imgView= [[UIImageView alloc]init];
        CGRect frame;
        frame.origin.x = self.scrView.frame.size.width * i;
        frame.origin.y = 0;
        frame.size = self.scrView.frame.size;
         self.imgView.frame = frame;
        self.imgView.image = [images objectAtIndex:i];
        [self.scrView addSubview:self.imgView];
    }
    self.scrView.contentSize = CGSizeMake(self.scrView.frame.size.width * images.count, self.scrView.frame.size.height);
    
    
    UISwipeGestureRecognizer *recognizer = [[UISwipeGestureRecognizer alloc] initWithTarget:self action:@selector(handleSwipeFrom:)];
    [recognizer setDirection:(UISwipeGestureRecognizerDirectionRight)];
    [self.scrView addGestureRecognizer:recognizer];
    
    
    recognizer = [[UISwipeGestureRecognizer alloc] initWithTarget:self action:@selector(handleSwipeFrom:)];
    [recognizer setDirection:(UISwipeGestureRecognizerDirectionLeft)];
    [self.scrView addGestureRecognizer:recognizer];

}

-(void)handleSwipeFrom :(UISwipeGestureRecognizer*)swipegesture {
    if([swipegesture direction] == UISwipeGestureRecognizerDirectionLeft)
    {
        [self nextButtonClicked:nil];
    }
    else{
        [self prevButtonClicked:nil];
    }
}

- (IBAction)prevButtonClicked:(id)sender{
    if(indexOfArticle > 0){
        indexOfArticle = indexOfArticle-1;
        [self.scrView setContentOffset:CGPointMake(320*indexOfArticle, 0) animated:YES];
    }
}
- (IBAction)nextButtonClicked:(id)sender{
    if(indexOfArticle < images.count-1){
        indexOfArticle = indexOfArticle+1;
        [self.scrView setContentOffset:CGPointMake(320*indexOfArticle, 0) animated:YES];
    }
}


- (IBAction)homeButtonTapped:(id)sender {
    [self.navigationController popViewControllerAnimated:YES];
}
@end
