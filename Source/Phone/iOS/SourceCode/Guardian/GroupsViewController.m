//
//  GroupsViewController.m
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "GroupsViewController.h"
#import "RegisterViewController.h"
#import "LiveDetails.h"
#import "SearchGroupsCustomCell.h"
#import "GroupCustomcell.h"

@interface GroupsViewController ()

@property (nonatomic , retain) IBOutlet UIView *viewMicrosoftTile;


@property (nonatomic , retain) IBOutlet UITableView *tbleMainGroups;
@property (nonatomic , retain) IBOutlet UITableView *tbleViewGroups;
@property (nonatomic , retain) IBOutlet UIView *viewInnerGroupList;
//@property (nonatomic , retain) IBOutlet UISearchBar *searchBar;

@property (nonatomic , retain) IBOutlet UITextField *txtSearch;

@property (nonatomic , retain) IBOutlet UIView *viewEnterMail;

@property (nonatomic , retain) IBOutlet UITextField *txtMailId;
@property (nonatomic , retain) IBOutlet UIButton *btnAdd;

@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect1;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect2;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect3;
@property (nonatomic , weak) IBOutlet UILabel *lblAddBuddy;
@property (nonatomic , weak) IBOutlet UILabel *lbltag1;
@property (nonatomic , weak) IBOutlet UILabel *lbltag2;
@property (nonatomic , weak) IBOutlet UILabel *lblEnterEmailIdText;
@property (nonatomic , retain) IBOutlet UIButton *btnPrivacy;

-(IBAction)btnPrivacyPoilcyClicked:(id)sender;

-(IBAction)searchClicked:(id)sender;
-(IBAction)addGroupClicked:(id)sender;
-(IBAction)btnAddClicked:(id)sender;
-(IBAction)closeView:(id)sender;
@end




@implementation GroupsViewController

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
    
    
    self.viewInnerGroupList.layer.borderWidth =1.5f;
    self.viewInnerGroupList.layer.borderColor = [UIColor whiteColor].CGColor;
    self.viewEnterMail.layer.borderWidth =1.5f;
    self.viewEnterMail.layer.borderColor = [UIColor whiteColor].CGColor;
    
    self.btnAdd.layer.borderWidth =1.5f;
    self.btnAdd.layer.borderColor = [UIColor whiteColor].CGColor;
    
    self.tbleViewGroups.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    self.tbleMainGroups.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    self.arrGroupsList = [[NSMutableArray alloc] init];
    self.arrSavedGroupsList = [[NSMutableArray alloc] init];

    
    
//    UITapGestureRecognizer * tapGesture = [[UITapGestureRecognizer alloc]
//                                           initWithTarget:self
//                                           action:@selector(hideView)];
//    
//    [self.view addGestureRecognizer:tapGesture];
    [self setfontForlabels];
    // Do any additional setup after loading the view from its nib.
}

-(void)viewWillAppear:(BOOL)animated{
//    [super viewWillAppear:animated];
//    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
//        
//        [self.viewMicrosoftTile setFrame:CGRectMake(0, 10, [UIScreen mainScreen].applicationFrame.size.width,[UIScreen mainScreen].applicationFrame.size.height-65)];
//        [self.view addSubview:self.viewMicrosoftTile];
//        //        [self.scrlView removeFromSuperview];
//    }
//    else{
//        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
//        if([defaults boolForKey:@"ProfileInserted"]){
//            [self updateMaingroupTable];
//        }
//        else{
//            [self.viewMicrosoftTile setFrame:CGRectMake(0, 10, [UIScreen mainScreen].applicationFrame.size.width,[UIScreen mainScreen].applicationFrame.size.height-65)];
//            [self.view addSubview:self.viewMicrosoftTile];
//        }
//    }
}

-(void)viewDidAppear:(BOOL)animated{
    [super viewDidAppear:animated];
    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
        
        [self.viewMicrosoftTile setFrame:CGRectMake(0, 10, [UIScreen mainScreen].applicationFrame.size.width,[UIScreen mainScreen].applicationFrame.size.height-65)];
        [self.view addSubview:self.viewMicrosoftTile];
        //        [self.scrlView removeFromSuperview];
    }
    else{
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        if([defaults boolForKey:@"ProfileInserted"]){
            [self updateMaingroupTable];
        }
        else{
            [self.viewMicrosoftTile setFrame:CGRectMake(0, 10, [UIScreen mainScreen].applicationFrame.size.width,[UIScreen mainScreen].applicationFrame.size.height-65)];
            [self.view addSubview:self.viewMicrosoftTile];
        }
    }
}

-(void)setfontForlabels {
    
    self.txtMailId.font = [UIFont fontWithName:@"SegoeUI" size:self.txtMailId.font.pointSize];
    self.btnAdd.titleLabel.font = [UIFont fontWithName:@"SegoeUI" size:self.btnAdd.titleLabel.font.pointSize];
    self.lblLiveConnect1.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect1.font.pointSize];
    self.lblLiveConnect2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect2.font.pointSize];
    self.lblLiveConnect3.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect3.font.pointSize];
    self.lblAddBuddy.font = [UIFont fontWithName:@"SegoeUI" size:self.lblAddBuddy.font.pointSize];
    self.lbltag1.font = [UIFont fontWithName:@"SegoeUI" size:self.lbltag1.font.pointSize];
    self.lbltag2.font = [UIFont fontWithName:@"SegoeUI" size:self.lbltag2.font.pointSize];
    self.txtMailId.font = [UIFont fontWithName:@"SegoeUI" size:self.txtMailId.font.pointSize];
    self.btnPrivacy.titleLabel.font = [UIFont fontWithName:@"SegoeUI" size:self.btnPrivacy.titleLabel.font.pointSize];
    self.lblEnterEmailIdText.font = [UIFont fontWithName:@"SegoeUI" size:self.lblEnterEmailIdText.font.pointSize];
}

-(UIViewController*) getRootViewController {
    return [UIApplication sharedApplication].keyWindow.rootViewController;
}


-(IBAction)closeView:(id)sender{
    [self.viewInnerGroupList removeFromSuperview];
    [self.viewEnterMail removeFromSuperview];
}

-(void)showEmailView{
    
    [self.viewEnterMail setFrame:CGRectMake(10, 25, 300, 350)];
    [self.view addSubview:self.viewEnterMail];
    
    [self.viewInnerGroupList removeFromSuperview];
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

-(IBAction)searchClicked:(id)sender{
    [self searchInServerAndIndex:0];
}

-(void)searchInServerAndIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        @try {
            if([[GlobalClass sharedInstance] connected]){
                
                [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                                  KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                                  KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
                
                NSString *str ;
                if(self.txtSearch.text.length == 0){
                    str = @"all";
                }
                else{
                    str = self.txtSearch.text;
                }
                
                
                NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:[NSString stringWithFormat:@"%@%@",[NSString stringWithFormat:@"%@",kGetListOfGroups],str]] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
                [request1 setHTTPMethod:@"GET"];
                [request1 setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
                
                [NSURLConnection sendAsynchronousRequest:request1
                                                   queue:[[NSOperationQueue alloc] init]
                                       completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                           if(!error && data){
                                               id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                               NSLog(@"%@",object);
                                               
                                               self.arrGroupsList = [[object objectForKey:@"List"] mutableCopy];
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   [self.txtSearch resignFirstResponder];
                                                   [self.tbleViewGroups reloadData];
                                               });
                                           }
                                           else{
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   [self searchInServerAndIndex:(NoOfTimes+1)];
                                               });
                                           }
                                           
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               // Update the UI
                                               [KVNProgress dismiss];
                                           });
                                       }];
            }
            
            else{
//                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//                [alert show];
            }
        }
        @catch (NSException *exception) {
            [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
            NSLog(@"%@",exception);
        }
        @finally {
            
        }
    }
    
}

-(IBAction)addGroupClicked:(id)sender{
    [self.viewInnerGroupList setFrame:CGRectMake(10, 25, 300, 350)];
    [self.view addSubview:self.viewInnerGroupList];
    [self.viewMicrosoftTile removeFromSuperview];

}

-(IBAction)btnAddClicked:(id)sender{
    @try {
        BOOL isValid =NO;
        if(self.txtMailId.text.length > 0){
            if(([[[self.arrGroupsList objectAtIndex:selectedIndex] objectForKey:@"Type"] integerValue] == 0)){
                isValid = YES;
                [[DBaseInteraction sharedInstance] InsertGroupData:[self.arrGroupsList objectAtIndex:selectedIndex] andEnrollmentValue:@""];
                
                [self.viewInnerGroupList setFrame:CGRectMake(10, 25, 300, 350)];
                [self.view addSubview:self.viewInnerGroupList];
                [self.viewEnterMail removeFromSuperview];
                
                [self updateMaingroupTable];
                
            }
            else{
                if(([[[self.arrGroupsList objectAtIndex:selectedIndex] objectForKey:@"EnrollmentType"] integerValue] == 1)){
                    if([[GlobalClass sharedInstance] emailValidation:self.txtMailId.text]){
                        NSString *str = [[self.txtMailId.text componentsSeparatedByString:@"@"] objectAtIndex:1];
                        str = [NSString stringWithFormat:@"@%@",str];
                        if([str isEqualToString:[[self.arrGroupsList objectAtIndex:selectedIndex] objectForKey:@"EnrollmentKey"]]){
                            isValid = YES;
                        }
                    }
                    else{
                        if([self.txtMailId.text rangeOfString:[[self.arrGroupsList objectAtIndex:selectedIndex] objectForKey:@"EnrollmentKey"]].location!=NSNotFound){
                            isValid = YES;
                        }
                    }
                }
                else{
                    isValid = YES;
                }
                
                if(isValid){
//                    isValid = YES;
                    [[DBaseInteraction sharedInstance] InsertGroupData:[self.arrGroupsList objectAtIndex:selectedIndex] andEnrollmentValue:self.txtMailId.text];
                    
                    [self.viewInnerGroupList setFrame:CGRectMake(10, 25, 300, 350)];
                    [self.view addSubview:self.viewInnerGroupList];
                    [self.viewEnterMail removeFromSuperview];
                    
                    [self updateMaingroupTable];
                }
                
            }
            
            AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
            appdele.settingChanged = YES;
        }
        
        if(!isValid){
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Please enter valid E-mail Id" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
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

-(void)updateMaingroupTable{
    @try {
        self.arrSavedGroupsList = [[[DBaseInteraction sharedInstance] getGroups] mutableCopy];
        [self.tbleMainGroups reloadData];
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}

#pragma mark UITableViewCells -UITableViewDelegate

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section;
{
    if(tableView == self.tbleViewGroups){
        return self.arrGroupsList.count;
    }
    else{
       return self.arrSavedGroupsList.count;
    }
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath{
    
    
    if(tableView == self.tbleViewGroups){
        static NSString *cellIdentifier=@"Cell";
        SearchGroupsCustomCell *cell=(SearchGroupsCustomCell *)[tableView dequeueReusableCellWithIdentifier:cellIdentifier];
        if (cell==nil) {
            NSArray *array=[[NSBundle mainBundle]loadNibNamed:@"SearchGroupsCustomCell" owner:self options:nil];
            cell=(SearchGroupsCustomCell *)[array objectAtIndex:0];
        }
        cell.selectionStyle = UITableViewCellSelectionStyleNone;
        
        cell.lblName.text = [[self.arrGroupsList objectAtIndex:indexPath.row] objectForKey:@"GroupName"];
        [cell.btnAdd addTarget:self action:@selector(accessoryButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
        cell.btnAdd.tag = indexPath.row;
        
        
        return cell;
    }
    else{
        static NSString *cellIdentifier=@"SelCell";
        GroupCustomcell *cell=(GroupCustomcell *)[tableView dequeueReusableCellWithIdentifier:cellIdentifier];
        if (cell==nil) {
            NSArray *array=[[NSBundle mainBundle]loadNibNamed:@"GroupCustomcell" owner:self options:nil];
            cell=(GroupCustomcell *)[array objectAtIndex:0];
        }
        cell.selectionStyle = UITableViewCellSelectionStyleNone;
        
        
        NSString *strName;
        strName = [NSString stringWithFormat:@"%@",[[self.arrSavedGroupsList objectAtIndex:indexPath.row] objectForKey:@"Name"]];
        if(([[[self.arrSavedGroupsList objectAtIndex:indexPath.row] objectForKey:@"IsValidated"] integerValue] != 1)){
            strName = [NSString stringWithFormat:@"*%@",strName];
        }
        
        cell.lblName.text = strName;
        cell.lblMobileNumber.text = [[self.arrSavedGroupsList objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"];
        cell.lblEmail.text = [[self.arrSavedGroupsList objectAtIndex:indexPath.row] objectForKey:@"Email"];
        
        [cell.btnDelete addTarget:self action:@selector(accessoryDeleteButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
        cell.btnDelete.tag = indexPath.row;
        

        return cell;
    }
    
    
}


- (void) accessoryButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    NSIndexPath * indexPath = [self.tbleViewGroups indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tbleViewGroups]];
    if ( indexPath == nil )
        return;
    
    [self.tbleViewGroups.delegate tableView:self.tbleViewGroups accessoryButtonTappedForRowWithIndexPath:indexPath];
}

- (void) accessoryDeleteButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    NSIndexPath * indexPath = [self.tbleMainGroups indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tbleMainGroups]];
    if ( indexPath == nil )
        return;
    
    [self.tbleMainGroups.delegate tableView:self.tbleMainGroups accessoryButtonTappedForRowWithIndexPath:indexPath];
}



- (void)tableView:(UITableView *)tableView accessoryButtonTappedForRowWithIndexPath:(NSIndexPath *)indexPath{
    NSLog(@"%ld",(long)indexPath.row);
    if(tableView == self.tbleViewGroups){
        selectedIndex = indexPath.row;
        
        NSArray *filteredarray = [self.arrSavedGroupsList filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(GroupId == %@)", [[self.arrGroupsList objectAtIndex:indexPath.row] objectForKey:@"GroupID"]]];
        if(filteredarray.count>0){
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Already group exists" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
            [alert show];
        }
        else{
            if(([[[self.arrGroupsList objectAtIndex:selectedIndex] objectForKey:@"Type"] integerValue] == 0)){
                [[DBaseInteraction sharedInstance] InsertGroupData:[self.arrGroupsList objectAtIndex:selectedIndex] andEnrollmentValue:@""];
                
                [self.viewInnerGroupList setFrame:CGRectMake(10, 25, 300, 350)];
                [self.view addSubview:self.viewInnerGroupList];
                [self.viewEnterMail removeFromSuperview];
                
                [self updateMaingroupTable];
                
            }
            else{
                if(([[[self.arrGroupsList objectAtIndex:selectedIndex] objectForKey:@"EnrollmentType"] integerValue] == 1)){
                    [self showEmailView];
                }
                else{
                    [[DBaseInteraction sharedInstance] InsertGroupData:[self.arrGroupsList objectAtIndex:selectedIndex] andEnrollmentValue:@""];
                    
                    [self.viewInnerGroupList setFrame:CGRectMake(10, 25, 300, 350)];
                    [self.view addSubview:self.viewInnerGroupList];
                    [self.viewEnterMail removeFromSuperview];
                    
                    [self updateMaingroupTable];
                }
                
            }
        }
    }
    else{
        @try {
            [[DBaseInteraction sharedInstance] DeleteGroupEdit:[NSString stringWithFormat:@"%@",[[self.arrSavedGroupsList objectAtIndex:indexPath.row] objectForKey:@"GroupId"]]];
        }
        @catch (NSException *exception) {
            [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
            NSLog(@"%@",exception);
        }
        @finally {
            
        }
        
        [self updateMaingroupTable];
        AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
        appdele.settingChanged = YES;
        NSLog(@"%ld",(long)indexPath.row);
    }
}



- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    if(tableView == self.tbleViewGroups){
        return 35;
    }
    else{
        return 70;
    }
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    if(tableView == self.tbleViewGroups){
        selectedIndex = indexPath.row;
        
        NSArray *filteredarray = [self.arrSavedGroupsList filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(GroupId == %@)", [[self.arrGroupsList objectAtIndex:indexPath.row] objectForKey:@"GroupID"]]];
        if(filteredarray.count>0){
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Already group exists" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
            [alert show];
        }
        else{
            [self showEmailView];
        }
    }
}

#pragma mark UITextField Delegate Methods
- (BOOL)textFieldShouldReturn:(UITextField *)textField{
    [textField resignFirstResponder];
    return YES;
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
