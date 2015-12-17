//
//  HelpRouteViewController.m
//  Guardian
//
//  Created by PTG on 10/03/15.
//  Copyright (c) 2015 People Tech Group. All rights reserved.
//

#import "HelpRouteViewController.h"

@interface HelpRouteViewController ()
@property (nonatomic,retain) IBOutlet UIWebView *webview;
-(IBAction)btnBackClicked:(id)sender;

@end

@implementation HelpRouteViewController
@synthesize webview = _webview;
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
    arrRoutePoints = [[NSMutableArray alloc] init];
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arrRoutePoints = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    // Do any additional setup after loading the view from its nib.
}
-(void) viewWillAppear:(BOOL)animated
{
	[super viewWillAppear:animated];
    [self.webview loadRequest:[NSURLRequest requestWithURL:[NSURL fileURLWithPath:[[NSBundle mainBundle]pathForResource:@"map" ofType:@"html"]isDirectory:NO]]];
}

- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType {
    
    NSString *triggerString=[[request URL] absoluteString];
    if([triggerString isEqualToString:@"ios:mapLoaded"]) {
        GeoTag *obj = (GeoTag *)[arrRoutePoints lastObject];
        [self performSelector:@selector(drawRoute:) withObject:obj afterDelay:1];
//        NSString *str= [NSString stringWithFormat:@"setGPSLocation('%@','%@','%@')",obj.Lati,obj.Longi,@"true"];
//        [self.webview stringByEvaluatingJavaScriptFromString:str];
    }
//    else if ([triggerString isEqualToString:@"ios:createRouteFromLocationsArray"]){
//        if(arrRoutePoints.count>0){
//            GeoTag *obj = (GeoTag *)[arrRoutePoints lastObject];
//            NSString *str= [NSString stringWithFormat:@"createRouteToDestLocArray('%@','%@')",obj.Lati,obj.Longi];
//            [self.webview stringByEvaluatingJavaScriptFromString:str];
//        }
//    }
//    else{
//        if(arrRoutePoints.count>0){
//            GeoTag *obj = (GeoTag *)[arrRoutePoints lastObject];
//            [self performSelector:@selector(drawRoute:) withObject:obj afterDelay:1];
//        }
//    }
    return YES;
    
}
-(void)drawRoute:(GeoTag *)obj{
    NSString *str= [NSString stringWithFormat:@"createRouteToDestLocation('%@','%@','%@','%@')",obj.Lati,obj.Longi,[NSString stringWithFormat:@"%f",self.locA.coordinate.latitude],[NSString stringWithFormat:@"%f",self.locA.coordinate.longitude]];
    [self.webview stringByEvaluatingJavaScriptFromString:str];
}

-(IBAction)btnBackClicked:(id)sender{
    [self.navigationController popViewControllerAnimated:YES];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
