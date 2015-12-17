//
//  LocalHelpViewController.m
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "LocalHelpViewController.h"
#import "LocalHelpCustomCell.h"
#import "HelpRouteViewController.h"

@interface LocalHelpViewController ()
@property (nonatomic ,weak) IBOutlet UITableView *tblLocalHelp;
@end

@implementation LocalHelpViewController
@synthesize arrAllList = _arrAllList;
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
    lat = @"";
    longi = @"";
    [self updateLocation];
    self.arrAllList = [[NSMutableArray alloc] init];
    arrPoliceStns = [[NSMutableArray alloc] init];;
    arrHospitals =[[NSMutableArray alloc] init];
//    [self getPolicestations];
    // Do any additional setup after loading the view from its nib.
}


-(void)updateLocation{
    NSMutableArray *arr= [[NSMutableArray alloc] init];
    NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
    if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
        arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
    }
    
    if(arr.count>0){
        
        GeoTag *obj = (GeoTag *)[arr lastObject];
        lat = obj.Lati;
        longi = obj.Longi;
        @try {
            [self getPoliceStations];
        }
        @catch (NSException *exception) {
            [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
            NSLog(@"%@",exception);
        }
        @finally {
            
        }
        
        NSLog(@"%@  %@",lat , longi);
        
    }
    
//    locationManager = appDel.locationManager;
//    locationManager.delegate = self;
//    locationManager.distanceFilter = kCLDistanceFilterNone;
//    locationManager.desiredAccuracy = kCLLocationAccuracyBest;
//    [locationManager startUpdatingLocation];
}

-(void)getPoliceStations{
    
    if([[GlobalClass sharedInstance] connected]){
        [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                          KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                          KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
        NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:[NSString stringWithFormat:@"http://dev.virtualearth.net/services/v1/SearchService/SearchService.asmx/Search2?count=15&startingIndex=0&mapBounds=&locationcoordinates='%@'&entityType='Business'&sortorder=&query=&location=""&keyword='%@'&jsonso=r229&jsonp=microsoftMapsNetworkCallback&culture='en-us'&token=AoBFMSS4EOyLV9jxIidneive6OtB21mVyzr520OsUwO51tFxCe9vgShVsHs2rz7U",[NSString stringWithFormat:@"%@,%@",lat,longi],[NSString stringWithFormat:@"police+station"]]] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
        [request1 setHTTPMethod:@"GET"];
        
        [NSURLConnection sendAsynchronousRequest:request1
                                           queue:[[NSOperationQueue alloc] init]
                               completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                   if(!error && data){
                                       
                                       NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                       
                                       jsonString = [jsonString stringByReplacingOccurrencesOfString:@"microsoftMapsNetworkCallback("
                                                                                          withString:@""];
                                       jsonString = [jsonString stringByReplacingOccurrencesOfString:@".d},'r229');"
                                                                                          withString:@"}"];
                                       jsonString = [jsonString stringByReplacingOccurrencesOfString:@"E+"
                                                                                          withString:@""];
                                       
                                       
                                       NSError *error;
                                       NSData *da = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
                                       id json = [NSJSONSerialization JSONObjectWithData:da options:NSJSONReadingAllowFragments error:&error];
                                       arrPoliceStns = [[[[json objectForKey:@"response"] objectForKey:@"d"]objectForKey:@"SearchResults"] mutableCopy];
                                       self.arrAllList = [arrPoliceStns mutableCopy];
                                       
                                       NSLog(@"%@",json);
                                       NSLog(@"%@",error);
                                       
                                   }
                                   else{
                                       id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                       NSLog(@"%@",object);
                                   }
                                   dispatch_async(dispatch_get_main_queue(), ^{
                                       [self getHospitals];
                                   });
                                   
                               }];

    }
    else{
//        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//        [alert show];
    }
}

-(void)getHospitals{
    
    NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:[NSString stringWithFormat:@"http://dev.virtualearth.net/services/v1/SearchService/SearchService.asmx/Search2?count=15&startingIndex=0&mapBounds=&locationcoordinates='%@'&entityType='Business'&sortorder=&query=&location=""&keyword='%@'&jsonso=r229&jsonp=microsoftMapsNetworkCallback&culture='en-us'&token=AoBFMSS4EOyLV9jxIidneive6OtB21mVyzr520OsUwO51tFxCe9vgShVsHs2rz7U",[NSString stringWithFormat:@"%@,%@",lat,longi],[NSString stringWithFormat:@"hospitals"]]] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
    [request1 setHTTPMethod:@"GET"];
    
    [NSURLConnection sendAsynchronousRequest:request1
                                       queue:[[NSOperationQueue alloc] init]
                           completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                               if(!error && data){
                                   
                                   NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                   
                                   jsonString = [jsonString stringByReplacingOccurrencesOfString:@"microsoftMapsNetworkCallback("
                                                                                      withString:@""];
                                   jsonString = [jsonString stringByReplacingOccurrencesOfString:@".d},'r229');"
                                                                                      withString:@"}"];
                                   jsonString = [jsonString stringByReplacingOccurrencesOfString:@"E+"
                                                                                      withString:@""];
                                   
                                   
                                   NSError *error;
                                   NSData *da = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
                                   id json = [NSJSONSerialization JSONObjectWithData:da options:NSJSONReadingAllowFragments error:&error];
                                   
                                   
                                   arrHospitals = [[[[json objectForKey:@"response"] objectForKey:@"d"]objectForKey:@"SearchResults"] mutableCopy];
                                   
                                   for (id object in arrHospitals)
                                   {
                                       [self.arrAllList removeObject:object];  // make sure you don't add it if it's already there.
                                       [self.arrAllList addObject:object];
                                   }
                                   
                                   
                                   dispatch_async(dispatch_get_main_queue(), ^{
                                       [self.tblLocalHelp reloadData];
                                   });
                                   
                               }
                               else{
                                   dispatch_async(dispatch_get_main_queue(), ^{
                                   });
                                   id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                   NSLog(@"%@",object);
                               }
                               dispatch_async(dispatch_get_main_queue(), ^{
                                   // Update the UI
                                   [KVNProgress dismiss];
                               });
                           }];
}


#pragma mark UITableViewCells -UITableViewDelegate

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section;
{
    return self.arrAllList.count;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath{
    
    static NSString *cellIdentifier=@"Cell";
    LocalHelpCustomCell *cell=(LocalHelpCustomCell *)[tableView dequeueReusableCellWithIdentifier:cellIdentifier];
    if (cell==nil) {
        NSArray *array=[[NSBundle mainBundle]loadNibNamed:@"LocalHelpCustomCell" owner:self options:nil];
        cell=(LocalHelpCustomCell *)[array objectAtIndex:0];
    }
    
    cell.selectionStyle = UITableViewCellSelectionStyleNone;
    
    if ([[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Name"] rangeOfString:@"Police"].location != NSNotFound) {
        cell.imgprfl.image = [UIImage imageNamed:@"police.png"];
    } else {
        cell.imgprfl.image = [UIImage imageNamed:@"medical.png"];
    }
    
    NSString *addStr = [NSString stringWithFormat:@"%@,%@,%@,%@,%@",[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Address"],[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"City"],[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"PostalCode"],[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Country"],[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Phone"]];
    
    
    float Height = 2;
    
    float labelHeight1 = [self calculateHeight:[UIFont fontWithName:@"SegoeUI" size:12.0] andWidth:220 andText:[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Name"]];
    [cell.lblName setFrame:CGRectMake(cell.lblName.frame.origin.x, cell.lblName.frame.origin.y, cell.lblName.frame.size.width, labelHeight1)];
    
    Height = labelHeight1+1;
    
    float labelHeight2 = [self calculateHeight:[UIFont fontWithName:@"SegoeUI" size:12.0] andWidth:220 andText:addStr];
    
    Height = Height + labelHeight2 + 2;
    
    [cell.lblAddress setFrame:CGRectMake(cell.lblAddress.frame.origin.x,labelHeight1+1, cell.lblAddress.frame.size.width, labelHeight2)];
    [cell.viewBG setFrame:CGRectMake(cell.viewBG.frame.origin.x, cell.viewBG.frame.origin.y, cell.viewBG.frame.size.width, Height)];
    [cell.btnCall setFrame:CGRectMake(cell.btnCall.frame.origin.x, cell.btnCall.frame.origin.y, cell.btnCall.frame.size.width, Height)];
    
    labelHeight2 = (Height-30)/2;
    
    [cell.imgprfl setFrame:CGRectMake(cell.imgprfl.frame.origin.x, labelHeight2, cell.imgprfl.frame.size.width, cell.imgprfl.frame.size.height)];
    [cell.imgCall setFrame:CGRectMake(cell.imgCall.frame.origin.x, labelHeight2, cell.imgCall.frame.size.width, cell.imgCall.frame.size.height)];
    
    cell.lblName.font = [UIFont fontWithName:@"SegoeUI" size:12.0];
    cell.lblAddress.font = [UIFont fontWithName:@"SegoeUI" size:12.0];
    cell.lblName.text = [[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Name"];
    
    cell.lblAddress.text = addStr;
    
    [cell.btnCall addTarget:self action:@selector(accessoryCallButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
    cell.btnCall.tag = indexPath.row;
    
    
    cell.viewBG.layer.borderWidth =1.0f;
    cell.viewBG.layer.borderColor = [UIColor whiteColor].CGColor;
    
    cell.btnCall.layer.borderWidth =1.0f;
    cell.btnCall.layer.borderColor = [UIColor whiteColor].CGColor;
    
    
    return cell;
    
}


- (void) accessoryCallButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    
    @try {
        NSIndexPath * indexPath = [self.tblLocalHelp indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tblLocalHelp]];
        if ( indexPath == nil )
            return;
        UIDevice *device = [UIDevice currentDevice];
        
        NSString *cellNameStr = [NSString stringWithFormat:@"%@",[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Phone"]];
        cellNameStr =  [[cellNameStr componentsSeparatedByCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] componentsJoinedByString:@""];
        
        // Create character set with specified characters
        NSMutableCharacterSet *characterSet =
        [NSMutableCharacterSet characterSetWithCharactersInString:@"()-"];
        
        // Build array of components using specified characters as separtors
        NSArray *arrayOfComponents = [cellNameStr componentsSeparatedByCharactersInSet:characterSet];
        
        // Create string from the array components
        cellNameStr = [arrayOfComponents componentsJoinedByString:@""];
        
        
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


- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    float Height = 2;
    
    float labelHeight1 = [self calculateHeight:[UIFont fontWithName:@"SegoeUI" size:12.0] andWidth:220 andText:[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Name"]];
    
    Height = labelHeight1+1;
    
    NSString *addStr = [NSString stringWithFormat:@"%@,%@,%@,%@,%@",[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Address"],[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"City"],[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"PostalCode"],[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Country"],[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Phone"]];
    
    float labelHeight2 = [self calculateHeight:[UIFont fontWithName:@"SegoeUI" size:12.0] andWidth:220 andText:addStr];
    
    Height = Height + labelHeight2 + 5;
    
    return Height;
}

-(float)calculateHeight:(UIFont *)font andWidth:(float )myWidth andText:(NSString *)str{
    NSAttributedString *attributedText = [[NSAttributedString alloc] initWithString:str attributes:@{
                                                                                                     NSFontAttributeName:font
                                                                                                     }];
    CGRect rect = [attributedText boundingRectWithSize:(CGSize){myWidth, CGFLOAT_MAX}
                                               options:NSStringDrawingUsesLineFragmentOrigin
                                               context:nil];
    return rect.size.height;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    @try {
        CLLocation *loc = [[CLLocation alloc] initWithLatitude:[[[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Location"] objectForKey:@"Latitude"] doubleValue] longitude:[[[[self.arrAllList objectAtIndex:indexPath.row] objectForKey:@"Location"] objectForKey:@"Longitude"] doubleValue]];
        HelpRouteViewController *objHelpRouteViewController = [[HelpRouteViewController alloc] initWithNibName:@"HelpRouteViewController" bundle:nil];
        objHelpRouteViewController.locA = loc;
        [self.navigationController pushViewController:objHelpRouteViewController animated:YES];
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
