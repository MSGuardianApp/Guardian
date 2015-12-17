//
//  BuddiesViewController.m
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "BuddiesViewController.h"
#import "BuddiesCustomCell.h"
#import "PhoneBuddy.h"
#import "BuddiesSelCell.h"
#import "NBPhoneNumber.h"

@interface BuddiesViewController ()


@property (nonatomic , retain) IBOutlet UITableView *tbleViewSelBuddies;

@property (nonatomic , retain) IBOutlet UIView *viewAddBuddy;
@property (nonatomic , retain) IBOutlet UIScrollView *scrlViewBuddy;
@property (nonatomic , retain) IBOutlet UITextField *txtName;
@property (nonatomic , retain) IBOutlet UITextField *txtEmail;
@property (nonatomic , retain) IBOutlet UITextField *txtPhone;
@property (nonatomic , retain) IBOutlet UITextField *txtContact;
@property (nonatomic , retain) IBOutlet UITextField *txtRegionCode;
@property (nonatomic , retain) IBOutlet UIButton *btnAddConfirm;


@property (nonatomic , retain) IBOutlet UITableView *tbleViewBuddies;
@property (nonatomic , retain) IBOutlet UIView *viewInnerBuddiesList;
@property (nonatomic , retain) IBOutlet UISearchBar *searchBar;

@property (nonatomic , weak) IBOutlet UILabel *lblAddBuddy;
@property (nonatomic , weak) IBOutlet UILabel *lbltag1;
@property (nonatomic , weak) IBOutlet UILabel *lbltag2;
@property (nonatomic , retain) IBOutlet UILabel *lblName;
@property (nonatomic , retain) IBOutlet UILabel *lblEmail;
@property (nonatomic , retain) IBOutlet UILabel *lblContact;



-(IBAction)contactClicked:(id)sender;
-(IBAction)closeClicked:(id)sender;
-(IBAction)addConfirmClicked:(id)sender;

-(IBAction)addBuddyClicked:(id)sender;
@end

@implementation BuddiesViewController

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
    
    arrContacts = [[NSMutableArray alloc] init];
    _arrContactsData = [[NSMutableArray alloc] init];
    _arrContactsCopy = [[NSMutableArray alloc] init];
    
    self.arrBuddiesList = [[NSMutableArray alloc] init];
    
    self.searchBar.layer.cornerRadius = 1.0f;
    
    
    self.btnAddConfirm.layer.borderWidth =1.5f;
    self.btnAddConfirm.layer.borderColor = [UIColor whiteColor].CGColor;
    
    self.txtContact.layer.borderWidth =1.0f;
    self.txtContact.layer.borderColor = [UIColor whiteColor].CGColor;
    
    self.scrlViewBuddy.layer.borderWidth =1.5f;
    self.scrlViewBuddy.layer.borderColor = [UIColor whiteColor].CGColor;
    
    self.viewInnerBuddiesList.layer.borderWidth =1.5f;
    self.viewInnerBuddiesList.layer.borderColor = [UIColor whiteColor].CGColor;
    
    
    self.tbleViewBuddies.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    self.tbleViewSelBuddies.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    self.txtRegionCode.text = [[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"];
    [self getBuddyList];
    [self setfontForlabels];
    is_Searching = NO;
    
	UITapGestureRecognizer * tapGesture = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(hideKeyBoard)];
    [self.scrlViewBuddy addGestureRecognizer:tapGesture];
    // Do any additional setup after loading the view from its nib.
}


-(void)setfontForlabels {
    
    self.lblAddBuddy.font = [UIFont fontWithName:@"SegoeUI" size:self.lblAddBuddy.font.pointSize];
    self.lbltag1.font = [UIFont fontWithName:@"SegoeUI" size:self.lbltag1.font.pointSize];
    self.lbltag2.font = [UIFont fontWithName:@"SegoeUI" size:self.lbltag2.font.pointSize];
    self.lblName.font = [UIFont fontWithName:@"SegoeUI" size:self.lblName.font.pointSize];
    self.lblEmail.font = [UIFont fontWithName:@"SegoeUI" size:self.lblEmail.font.pointSize];
    self.lblContact.font = [UIFont fontWithName:@"SegoeUI" size:self.lblContact.font.pointSize];
    self.txtName.font = [UIFont fontWithName:@"SegoeUI" size:self.txtName.font.pointSize];
    
    self.txtEmail.font = [UIFont fontWithName:@"SegoeUI" size:self.txtEmail.font.pointSize];
    self.txtPhone.font = [UIFont fontWithName:@"SegoeUI" size:self.txtPhone.font.pointSize];
    self.txtContact.font = [UIFont fontWithName:@"SegoeUI" size:self.txtContact.font.pointSize];
    self.txtRegionCode.font = [UIFont fontWithName:@"SegoeUI" size:self.txtRegionCode.font.pointSize];
    self.btnAddConfirm.titleLabel.font = [UIFont fontWithName:@"SegoeUI" size:self.btnAddConfirm.titleLabel.font.pointSize];
}

-(void)getBuddyList {
    
    self.arrBuddiesList = [[[DBaseInteraction sharedInstance] getBuddyData] mutableCopy];
    @try {
//        if(self.arrBuddiesList.count>0){
//            if(![[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] || [[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] == NULL || [[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] == nil){
//                [[NSUserDefaults standardUserDefaults] setObject:[[self.arrBuddiesList objectAtIndex:0] objectForKey:@"Name"] forKey:@"DefaultCaller"];
//            }
//        }
//		else{
//            [[NSUserDefaults standardUserDefaults] removeObjectForKey:@"DefaultCaller"];
//        }
        
        [self.tbleViewSelBuddies reloadData];
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}
#pragma mark IBAction Methods
#pragma mark

-(IBAction)contactClicked:(id)sender{
    if(arrContacts.count >0){
        
//        PhoneBuddy *objPhoneBuddy = [[PhoneBuddy alloc] init];
//        objPhoneBuddy = (PhoneBuddy *)[_arrContactsData objectAtIndex:0];
//        NSString *str = objPhoneBuddy.mobileNumber;
//        
//        popupContacts = [[UIActionSheet alloc] initWithTitle:nil delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil otherButtonTitles:str,nil];
//        
//        popupContacts.actionSheetStyle = UIActionSheetStyleBlackOpaque;
//        [popupContacts showInView:self.view];
    }
}
-(IBAction)closeClicked:(UIButton *)sender{
    if(sender.tag == 1){
        [self.viewAddBuddy removeFromSuperview];
    }
    else{
        [self.viewInnerBuddiesList removeFromSuperview];
    }
}
-(IBAction)addConfirmClicked:(id)sender{
    @try {
        if(arrContacts > 0 ){
            
            if(self.arrBuddiesList.count<5){
                DBaseInteraction *objDBaseInteraction = [DBaseInteraction sharedInstance];
                PhoneBuddy *buddy = objPhnBud;
                
                NSString *cellNameStr = self.txtPhone.text;
                cellNameStr =  [[cellNameStr componentsSeparatedByCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] componentsJoinedByString:@""];
                
                // Create character set with specified characters
                NSMutableCharacterSet *characterSet =
                [NSMutableCharacterSet characterSetWithCharactersInString:@"()-"];
                
                // Build array of components using specified characters as separtors
                NSArray *arrayOfComponents = [cellNameStr componentsSeparatedByCharactersInSet:characterSet];
                
                // Create string from the array components
                cellNameStr = [arrayOfComponents componentsJoinedByString:@""];
                
                cellNameStr = [cellNameStr stringByReplacingOccurrencesOfString:self.txtRegionCode.text withString:@""];
                
                if(cellNameStr.length == 11 && [cellNameStr hasPrefix:@"0"]){
                    cellNameStr = [cellNameStr substringFromIndex:1];
                }
                
                
                NBPhoneNumberUtil *phoneUtil = [[NBPhoneNumberUtil alloc] init];
                NSError *error = nil;
                
                
                NBPhoneNumber *phoneNumberUS = [phoneUtil parse:cellNameStr defaultRegion:[[NSUserDefaults standardUserDefaults] objectForKey:@"LocaleCode"] error:&error];
                if (error) {
                    NSLog(@"err [%@]", [error localizedDescription]);
                }
//                if([phoneUtil isValidNumberForRegion:phoneNumberUS regionCode:[[NSUserDefaults standardUserDefaults] objectForKey:@"LocaleCode"]]){
//                   ([phoneUtil isValidNumber:phoneNumberUS])
//                }
                if([phoneUtil isValidNumberForRegion:phoneNumberUS regionCode:[[NSUserDefaults standardUserDefaults] objectForKey:@"LocaleCode"]]){
                    
                    BOOL ToProceed = YES;
                    if([self.txtEmail.text length]>0){
                        ToProceed = [[GlobalClass sharedInstance] emailValidation:self.txtEmail.text];
                    }
                    if(ToProceed){
                        buddy.mobileNumber = [NSString stringWithFormat:@"%@%@",self.txtRegionCode.text,cellNameStr];
                        buddy.ToRemove = @"0";
                        buddy.BuddyId = @"0";
                        buddy.UserID = @"0";
                        buddy.Email = @"";
                        buddy.state = @"1";
                        if(self.arrBuddiesList.count==0){
                            buddy.IsPrimeBuddy=@"1";
                        }
                        else buddy.IsPrimeBuddy=@"0";
                        buddy.Email= self.txtEmail.text;
                        
                        [objDBaseInteraction InsertBuddyData:buddy];
                        
                        [self getBuddyList];
                        
                        [self addBuddyList];
                        [self.viewAddBuddy removeFromSuperview];
                        
                        AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
                        appdele.settingChanged = YES;
                        
//                        if(![[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] || [[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] == NULL || [[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] == nil){
//                            [[NSUserDefaults standardUserDefaults] setObject:[NSString stringWithFormat:@"%@ %@",buddy.firstName,buddy.lastName] forKey:@"DefaultCaller"];
//                        }
                    }
                    else{
                        [[[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Enter valid E-mail Id" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil] show];
                    }

                    
                }
                else{
                    UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Please Enter Valid Mobile Number" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                    [warningAlert show];
                }
                
//                if([cellNameStr length] == 10){
//                    
//                }
//                else{
//                    [[[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Enter valid Phone Number" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil] show];
//                }

            }
            
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}

-(IBAction)addBuddyClicked:(id)sender{
    @try {
        if(self.arrBuddiesList.count<5){
            if(_arrContactsData.count > 0){
                [self addBuddyList];
            }
            else{
                CFErrorRef error = nil;
                ABAddressBookRef addressBook = ABAddressBookCreateWithOptions(NULL, &error);
                if (!addressBook) // test the result, not the error
                {
                    NSLog(@"ERROR!!!");
                    return; // bail
                }
                ABAddressBookRequestAccessWithCompletion(addressBook, ^(bool granted, CFErrorRef error) {
                    if (error) {
                        NSLog(@"error %@", error);
                        
                    }else if (granted){
                        
                        NSArray *arrayOfPeople = (__bridge_transfer NSArray *)ABAddressBookCopyArrayOfAllPeople(addressBook);
                        
                        _arrContactsData = [[NSMutableArray alloc] init];
                        
                        for(NSUInteger index = 0; index < ([arrayOfPeople count]); index++){
                            
                            PhoneBuddy *objPhoneBuddy = [[PhoneBuddy alloc] init];
                            
                            ABRecordRef currentPerson = (__bridge ABRecordRef)[arrayOfPeople objectAtIndex:index];
                            NSString *str = (__bridge_transfer NSString *)ABRecordCopyValue(currentPerson, kABPersonFirstNameProperty);
                            
                            if(str)
                                objPhoneBuddy.firstName = str;
                            else
                                objPhoneBuddy.firstName = @"";
                            
                            str = (__bridge_transfer NSString *)ABRecordCopyValue(currentPerson, kABPersonLastNameProperty);
                            if(str)
                                objPhoneBuddy.lastName = str;
                            else
                                objPhoneBuddy.lastName = @"";
                            
                            UIImage* image;
                            
                            if(ABPersonHasImageData(currentPerson)){
                                image = [UIImage imageWithData:(__bridge NSData *)ABPersonCopyImageData(currentPerson)];
                            }else{
                                image = [UIImage imageNamed:@"add_user_icon.png"];
                            }
                            objPhoneBuddy.ProfilePic = image;
                            
                            //        str = (__bridge_transfer NSString *)ABRecordCopyValue(currentPerson, kABPersonEmailProperty);
                            //        if(str)
                            //            [dict setObject:str forKey:@"Email"];
                            //        else
                            //            [dict setObject:@"" forKey:@"Email"];
                            
                            ABMultiValueRef phonesRef = ABRecordCopyValue(currentPerson, kABPersonPhoneProperty);
                            for (int i=0; i<ABMultiValueGetCount(phonesRef); i++) {
                                CFStringRef currentPhoneLabel = ABMultiValueCopyLabelAtIndex(phonesRef, i);
                                CFStringRef currentPhoneValue = ABMultiValueCopyValueAtIndex(phonesRef, i);
                                if(currentPhoneLabel!=nil){
                                    if (CFStringCompare(currentPhoneLabel, kABPersonPhoneMobileLabel, 0) == kCFCompareEqualTo) {
                                        objPhoneBuddy.mobileNumber = (__bridge NSString *)currentPhoneValue;
                                    }
                                    else
                                        objPhoneBuddy.mobileNumber = @"";
                                    
                                    
                                    if (CFStringCompare(currentPhoneLabel, kABHomeLabel, 0) == kCFCompareEqualTo) {
                                        objPhoneBuddy.homeNumber = (__bridge NSString *)currentPhoneValue;
                                    }
                                    else
                                        objPhoneBuddy.homeNumber = @"";
                                }
                                
                                else{
                                    objPhoneBuddy.mobileNumber = @"";
                                    objPhoneBuddy.homeNumber = @"";
                                }
                                
                                CFRelease(currentPhoneValue);
                                
                            }
                            if([objPhoneBuddy.mobileNumber length]==0){
                                objPhoneBuddy.mobileNumber = objPhoneBuddy.homeNumber;
                            }
                            
                            ABMultiValueRef emailMultiValue = ABRecordCopyValue(currentPerson, kABPersonEmailProperty);
                            
                            NSString *email = @"";
                            
                            for (CFIndex i = 0; i < ABMultiValueGetCount(emailMultiValue); i++) {
                                NSString *label = (__bridge NSString *) ABMultiValueCopyLabelAtIndex(emailMultiValue, i);
                                if ([label isEqualToString:(NSString *)kABHomeLabel]) {
                                    if(email.length == 0)
                                        email = (__bridge NSString *) ABMultiValueCopyValueAtIndex(emailMultiValue, i);
                                    NSLog(@"%@",email);
                                }
                            }
                            CFRelease(emailMultiValue);
                            
                            objPhoneBuddy.Email = email;
                            
                            //            NSArray *emailAddresses = (__bridge NSArray *)ABMultiValueCopyArrayOfAllValues(emailMultiValue) ;
                            //
                            //            NSLog(@"%@",emailAddresses);
                            
                            //            CFRelease(emailMultiValue);
                            
                            [_arrContactsData addObject: objPhoneBuddy];
                        }
                        
                        
                        NSSortDescriptor *brandDescriptor = [[NSSortDescriptor alloc] initWithKey:@"firstName" ascending:YES] ;
                        NSArray *sortDescriptors = [NSArray arrayWithObject:brandDescriptor];
                        NSArray *sortedFirstNames = [_arrContactsData sortedArrayUsingDescriptors:sortDescriptors];
                        NSLog(@"%@",sortedFirstNames);
                        [_arrContactsData removeAllObjects];
                        _arrContactsData = [sortedFirstNames mutableCopy];
                        
                        dispatch_async(dispatch_get_main_queue(), ^{
                            [self addBuddyList];
                        });
                        // Do what you want with the Address Book
                        
                    }else{
                        NSLog(@"permission denied");
                        [[[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Contact access denied" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil] show];
                    }
                    
                    CFRelease(addressBook);
                });
                
                //
                //        CFArrayRef arrayOfPeople1 = ABAddressBookCopyArrayOfAllPeople(addressBook);
                //        NSLog(@"%@", arrayOfPeople1);
                
                //The array in which the first names will be stored as NSStrings
            }
        }
        else{
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"You have reached your limit of 5 buddies allowed for a profile.Please remove someone to add a new buddy" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
    
//    [self addBuddyList];
}

-(void)addScrlViewBuddyView:(NSInteger )index {
    if(_arrContactsData.count >0){
        
        CGRect mainFrame;
        CGSize result = [[UIScreen mainScreen] bounds].size;
        if(result.height == 480)
        {
            mainFrame = CGRectMake(0, 0, 320, 420);
        }
        if(result.height > 480)
        {
            mainFrame = CGRectMake(0, 0, 320, 508);
        }
        
        [self.viewAddBuddy setFrame:mainFrame];
        
        
        
        PhoneBuddy *objPhoneBuddy = [[PhoneBuddy alloc] init];
        if(!is_Searching)
        {
            objPhoneBuddy = (PhoneBuddy *)[_arrContactsData objectAtIndex:index];
        }
        else objPhoneBuddy = (PhoneBuddy *)[_arrContactsCopy objectAtIndex:index];
        
        objPhnBud = objPhoneBuddy;
        
        self.txtName.text = [NSString stringWithFormat:@"%@ %@",objPhoneBuddy.firstName,objPhoneBuddy.lastName];
        self.txtPhone.text = [NSString stringWithFormat:@"%@",objPhoneBuddy.mobileNumber];
        self.txtContact.text = [NSString stringWithFormat:@"%@",objPhoneBuddy.mobileNumber];
        [arrContacts addObject:objPhoneBuddy];
        
        [self.viewInnerBuddiesList removeFromSuperview];
        
        [self.view addSubview:self.viewAddBuddy];
        
    }
}

-(void)addBuddyList {
    CGRect mainFrame = CGRectMake(10, 06, 300, 372);
    [self.viewInnerBuddiesList setFrame:mainFrame];
    [self.view addSubview:self.viewInnerBuddiesList];
    let_User_SelectRow = YES;
    [self.tbleViewBuddies reloadData];
}

#pragma mark UIActionSheet Delegate Methods
#pragma mark

-(void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex {
    if(buttonIndex == 0){
    }
    else if (buttonIndex == 1){
    }
}





#pragma mark UITableViewCells -UITableViewDelegate

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section;
{
    if(tableView == self.tbleViewBuddies){
        if(!is_Searching)
        {
            return [_arrContactsData count];
        }
        else return [_arrContactsCopy count];
    }
    
    else if(tableView == self.tbleViewSelBuddies){
        return self.arrBuddiesList.count;
    }
        
    else return 0;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath{
    
    if(tableView == self.tbleViewBuddies){
        static NSString *cellIdentifier=@"Cell";
        BuddiesCustomCell *cell=(BuddiesCustomCell *)[tableView dequeueReusableCellWithIdentifier:cellIdentifier];
        if (cell==nil) {
            NSArray *array=[[NSBundle mainBundle]loadNibNamed:@"BuddiesCustomCell" owner:self options:nil];
            cell=(BuddiesCustomCell *)[array objectAtIndex:0];
        }
        PhoneBuddy *objPhoneBuddy = [[PhoneBuddy alloc] init];
        if(!is_Searching)
        {
            objPhoneBuddy = (PhoneBuddy *)[_arrContactsData objectAtIndex:indexPath.row];
        }
        else objPhoneBuddy = (PhoneBuddy *)[_arrContactsCopy objectAtIndex:indexPath.row];
        
        cell.selectionStyle = UITableViewCellSelectionStyleNone;
        
        NSString *strname = [NSString stringWithFormat:@"%@ %@",objPhoneBuddy.firstName,objPhoneBuddy.lastName];
        
        NSAttributedString *attributedText = [[NSAttributedString alloc] initWithString:strname attributes:@{
                                                                                                            NSFontAttributeName: [UIFont systemFontOfSize:18.0]
                                                                                                            }];
        CGRect rect = [attributedText boundingRectWithSize:(CGSize){196, CGFLOAT_MAX}
                                                   options:NSStringDrawingUsesLineFragmentOrigin
                                                   context:nil];
        [cell.lblName setFrame:CGRectMake(cell.lblName.frame.origin.x,cell.lblName.frame.origin.y, cell.lblName.frame.size.width, rect.size.height)];
        cell.lblName.text = strname;
        
        cell.imgprfl.image = (UIImage *)objPhoneBuddy.ProfilePic;
        
        [cell.btnAdd addTarget:self action:@selector(accessoryButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
        cell.btnAdd.tag = indexPath.row;

        
        return cell;

    }
    else if (tableView == self.tbleViewSelBuddies){
        
        static NSString *cellIdentifier=@"SelCell";
        BuddiesSelCell *cell1=(BuddiesSelCell *)[tableView dequeueReusableCellWithIdentifier:cellIdentifier];
        if (cell1==nil) {
            NSArray *array=[[NSBundle mainBundle]loadNibNamed:@"BuddiesSelCell" owner:self options:nil];
            cell1=(BuddiesSelCell *)[array objectAtIndex:0];
        }
        cell1.selectionStyle = UITableViewCellSelectionStyleNone;
        NSString *strName;
        strName = [NSString stringWithFormat:@"  %@",[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"Name"]];
        if([[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"State"] isEqualToString:@"2"]){
            strName = [NSString stringWithFormat:@"%@",[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"Name"]];
            strName = [NSString stringWithFormat:@"* %@",strName];
        }
        else if ([[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"State"] isEqualToString:@"4"]){
            strName = [NSString stringWithFormat:@"%@",[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"Name"]];
            strName = [NSString stringWithFormat:@"+ %@",strName];
        }
        
        cell1.lblName.text = strName;
        cell1.lblMobileNumber.text = [NSString stringWithFormat:@"  %@",[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]];
        cell1.lblEmail.text = [NSString stringWithFormat:@"  %@",[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"Email"]];
        [cell1.btnDelete addTarget:self action:@selector(accessoryDeleteButtonTapped:withEvent:) forControlEvents:UIControlEventTouchUpInside];
        cell1.btnDelete.tag = indexPath.row;
        
        [cell1.lblName setTextColor:[UIColor whiteColor]];
        [cell1.lblMobileNumber setTextColor:[UIColor whiteColor]];
        
//        if([[[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] isEqualToString:[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"Name"]]){
//            [cell1.lblName setTextColor:[UIColor purpleColor]];
//            [cell1.lblMobileNumber setTextColor:[UIColor purpleColor]];
//        }
//        else{
//            [cell1.lblName setTextColor:[UIColor whiteColor]];
//            [cell1.lblMobileNumber setTextColor:[UIColor whiteColor]];
//        }
        
        return cell1;
    }
    else return nil;
}

- (void) accessoryButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    NSIndexPath * indexPath = [self.tbleViewBuddies indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tbleViewBuddies]];
    if ( indexPath == nil )
        return;
    
    [self.tbleViewBuddies.delegate tableView:self.tbleViewBuddies accessoryButtonTappedForRowWithIndexPath:indexPath];
}

- (void) accessoryDeleteButtonTapped: (UIControl *) button withEvent: (UIEvent *) event
{
    NSIndexPath * indexPath = [self.tbleViewSelBuddies indexPathForRowAtPoint:[[[event touchesForView:button] anyObject] locationInView:self.tbleViewSelBuddies]];
    if ( indexPath == nil )
        return;
    
    [self.tbleViewSelBuddies.delegate tableView:self.tbleViewSelBuddies accessoryButtonTappedForRowWithIndexPath:indexPath];
}


- (void)tableView:(UITableView *)tableView accessoryButtonTappedForRowWithIndexPath:(NSIndexPath *)indexPath{
    
    if(tableView == self.tbleViewBuddies){
        if(self.arrBuddiesList.count<6){
            PhoneBuddy *objPhoneBuddy = [[PhoneBuddy alloc] init];
            if(!is_Searching)
            {
                objPhoneBuddy = (PhoneBuddy *)[_arrContactsData objectAtIndex:indexPath.row];
            }
            else
            {
                objPhoneBuddy = (PhoneBuddy *)[_arrContactsCopy objectAtIndex:indexPath.row];
            }
            NSString *cellNameStr = objPhoneBuddy.mobileNumber;
            // Create character set with specified characters
            NSMutableCharacterSet *characterSet =
            [NSMutableCharacterSet characterSetWithCharactersInString:@"()-"];
            
            // Build array of components using specified characters as separtors
            NSArray *arrayOfComponents = [cellNameStr componentsSeparatedByCharactersInSet:characterSet];
            
            // Create string from the array components
            cellNameStr = [arrayOfComponents componentsJoinedByString:@""];
            
            cellNameStr = [cellNameStr stringByReplacingOccurrencesOfString:self.txtRegionCode.text withString:@""];
            
            if(cellNameStr.length == 11 && [cellNameStr hasPrefix:@"0"]){
                cellNameStr = [cellNameStr substringFromIndex:1];
            }
            
            
            cellNameStr = [NSString stringWithFormat:@"%@%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"],cellNameStr];
            
            NSArray *filteredarray = [self.arrBuddiesList filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(PhoneNumber == %@)", cellNameStr]];
            if(filteredarray.count>0){
                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Already buddy exists" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
                [alert show];
            }
            else{
                [self addScrlViewBuddyView:indexPath.row];
            }
        }
        else{
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"You can't more than 5 buddies" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
            [alert show];
        }
        
    }
    else{
        if([[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"BuddyRelationshipId"] isEqualToString:@""] || [[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"BuddyRelationshipId"] isKindOfClass:[NSNull class]] ){
            [[DBaseInteraction sharedInstance] DeleteBuddyFromDB:[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]];
        }
        else{
            [[DBaseInteraction sharedInstance] DeleteBuddyEdit:[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"PhoneNumber"]];
        }
        
        if([[[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] isEqualToString:[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"Name"]]){
            if(self.arrBuddiesList.count>0){
                    [[NSUserDefaults standardUserDefaults] setObject:[[self.arrBuddiesList objectAtIndex:0] objectForKey:@"Name"] forKey:@"DefaultCaller"];
            }
            else{
                [[NSUserDefaults standardUserDefaults] removeObjectForKey:@"DefaultCaller"];
            }

        }
        
        AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
        appdele.settingChanged = YES;
        [self getBuddyList];
        NSLog(@"%ld",(long)indexPath.row);
    }
}



- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    if(tableView == self.tbleViewBuddies){
        float Height = 5;
        PhoneBuddy *objPhoneBuddy = [[PhoneBuddy alloc] init];
        
        if(!is_Searching)
        {
            objPhoneBuddy = (PhoneBuddy *)[_arrContactsData objectAtIndex:indexPath.row];
        }
        else objPhoneBuddy = (PhoneBuddy *)[_arrContactsCopy objectAtIndex:indexPath.row];
        
        NSString *strname = [NSString stringWithFormat:@"%@ %@",objPhoneBuddy.firstName,objPhoneBuddy.lastName];
        NSAttributedString *attributedText = [[NSAttributedString alloc] initWithString:strname attributes:@{
                                                                                                             NSFontAttributeName: [UIFont systemFontOfSize:18.0]
                                                                                                             }];
        CGRect rect = [attributedText boundingRectWithSize:(CGSize){196, CGFLOAT_MAX}
                                                   options:NSStringDrawingUsesLineFragmentOrigin
                                                   context:nil];
        
        Height= Height+rect.size.height;
        Height = Height+5;
        if(Height < 44){
            return 44;
        }
        
        return Height;

    }
    else if(tableView == self.tbleViewSelBuddies){
        return 77;
    }
    else return 0;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    if(tableView == self.tbleViewBuddies){
        
        if(self.arrBuddiesList.count<5){
            PhoneBuddy *objPhoneBuddy = [[PhoneBuddy alloc] init];
            if(!is_Searching)
            {
                objPhoneBuddy = (PhoneBuddy *)[_arrContactsData objectAtIndex:indexPath.row];
            }
            else
            {
                objPhoneBuddy = (PhoneBuddy *)[_arrContactsCopy objectAtIndex:indexPath.row];
            }
            NSString *cellNameStr = objPhoneBuddy.mobileNumber;
            // Create character set with specified characters
            NSMutableCharacterSet *characterSet =
            [NSMutableCharacterSet characterSetWithCharactersInString:@"()-"];
            
            // Build array of components using specified characters as separtors
            NSArray *arrayOfComponents = [cellNameStr componentsSeparatedByCharactersInSet:characterSet];
            
            // Create string from the array components
            cellNameStr = [arrayOfComponents componentsJoinedByString:@""];
            cellNameStr = [cellNameStr stringByReplacingOccurrencesOfString:self.txtRegionCode.text withString:@""];
            
            if(cellNameStr.length == 11 && [cellNameStr hasPrefix:@"0"]){
                cellNameStr = [cellNameStr substringFromIndex:1];
            }
            
            cellNameStr = [NSString stringWithFormat:@"%@%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"],cellNameStr];
            cellNameStr = [cellNameStr stringByReplacingOccurrencesOfString:@" " withString:@""];
            
            NSArray *filteredarray = [self.arrBuddiesList filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(PhoneNumber == %@)", cellNameStr]];
            if(filteredarray.count>0){
                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Already buddy exists" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
                [alert show];
            }
            else{
                [self addScrlViewBuddyView:indexPath.row];
            }
        }
        else{
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"You have reached your limit of 5 buddies allowed for a profile.Please remove someone to add a new buddy" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
        }
        
        
    }
    else if (tableView == self.tbleViewSelBuddies){
//        [[NSUserDefaults standardUserDefaults] setObject:[[self.arrBuddiesList objectAtIndex:indexPath.row] objectForKey:@"Name"] forKey:@"DefaultCaller"];
//        [self.tbleViewSelBuddies reloadData];
    }
}



#pragma mark -
#pragma mark SearchBar Delegate Method

- (void) searchBarTextDidBeginEditing:(UISearchBar *)theSearchBar
{
//    if(is_Searching)
//		return;
//    is_Searching = YES;
    self.searchBar.showsCancelButton = YES;
//	let_User_SelectRow = NO;
//    [self.tbleViewBuddies reloadData];
}

- (BOOL)searchBarShouldBeginEditing:(UISearchBar *)bar {
    // reset the shouldBeginEditing BOOL ivar to YES, but first take its value and use it to return it from the method call
    BOOL boolToReturn = let_User_SelectRow;
    let_User_SelectRow = YES;
//    is_Searching = YES;
    return boolToReturn;
}

- (void)searchBar:(UISearchBar *)theSearchBar textDidChange:(NSString *)searchText
{
    
    @try {
        if([searchText length] > 0)
        {
            is_Searching = YES;
            let_User_SelectRow = YES;
            
            NSMutableArray *searchArray = [[NSMutableArray alloc] initWithArray:_arrContactsData];
            
            [_arrContactsCopy removeAllObjects];
            
            for (int i =0; i< [searchArray count]; i++)
            {
                NSString *str_temp = @"";
                //            NSArray *Temp_cell_Array = [Dic_Relation_List objectForKey:[Array_Sections 	objectAtIndex:i]];
                //            if([Temp_cell_Array count]>0)
                //            {
                //                str_temp = [[[[Temp_cell_Array objectAtIndex:i] valueForKey:JSON_PARTPROS] objectAtIndex:0] valueForKey:JSON_PARTPROS_VAL0];
                //            }
                
                PhoneBuddy *objPhoneBuddy = [[PhoneBuddy alloc] init];
                objPhoneBuddy = (PhoneBuddy *)[_arrContactsData objectAtIndex:i];
                
                str_temp = [NSString stringWithFormat:@"%@",objPhoneBuddy.firstName];
                //            str_temp=[[searchArray objectAtIndex:i] valueForKey:@"voornaam"];
                
                NSRange range = [str_temp rangeOfString:searchText
                                                options:NSCaseInsensitiveSearch];
                
                if (range.location != NSNotFound)
                {
                    [_arrContactsCopy addObject:objPhoneBuddy];
                }
            }
            searchArray = nil;
        }
        else
        {
            is_Searching = NO;
            [self.searchBar resignFirstResponder];
            //        [self configureSections:_arrContactsData];
        }
        
        [self.tbleViewBuddies reloadData];
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}


- (void)searchBarTextDidEndEditing:(UISearchBar *)searchBar
{
//    is_Searching = NO;
//    self.searchBar.text = @"";
    self.searchBar.showsCancelButton = NO;
    
    
//    if([self.searchBar.text length]>0){
//        is_Searching = NO;
////        [self.tbleViewBuddies reloadData];
//    }
//    else{
//        is_Searching = NO;
//        self.searchBar.text = @"";
//        self.searchBar.showsCancelButton = NO;
////        [self.tbleViewBuddies reloadData];
//    }
}

- (void)searchBarCancelButtonClicked:(UISearchBar *)searchBar
{
    self.searchBar.text = @"";
    
    [self.searchBar resignFirstResponder];
    [self.tbleViewBuddies reloadData];
}
- (void) searchBarSearchButtonClicked:(UISearchBar *)theSearchBar
{
    [self.searchBar resignFirstResponder];
//    [self.tbleViewBuddies reloadData];
}

- (void)scrollViewWillBeginDragging:(UIScrollView *)scrollView
{
    [self.searchBar resignFirstResponder];
}


#pragma mark TextField delegate methods

-(void)hideKeyBoard{
    for(UITextField *t in self.scrlViewBuddy.subviews){
        [t resignFirstResponder];
        [self.scrlViewBuddy setContentOffset:CGPointMake(0, 0) animated:YES];
    }
}

- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString *)string
{
    if(textField.tag == 1){
        NSString *currentString = [textField.text stringByReplacingCharactersInRange:range withString:string];
        NSInteger length = [currentString length];
        if([string isEqualToString:@""]){
            return YES;
        }
        else if (length > 10) {
            return NO;
        }
    }
    return YES;
}

- (void)textFieldDidBeginEditing:(UITextField *)textField{
    if(textField.tag == 1){
        [self.scrlViewBuddy setContentOffset:CGPointMake(0, 50) animated:YES];
    }
    else if(textField.tag == 2){
        [self.scrlViewBuddy setContentOffset:CGPointMake(0, 100) animated:YES];
    }
}
- (void)textFieldDidEndEditing:(UITextField *)textField{
    [self.scrlViewBuddy setContentOffset:CGPointMake(0, 0) animated:YES];
}

-(BOOL)textFieldShouldReturn:(UITextField *)textField{
    [textField resignFirstResponder];
    return YES;
}


- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
