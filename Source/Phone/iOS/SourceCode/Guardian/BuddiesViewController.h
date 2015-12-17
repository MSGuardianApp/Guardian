//
//  BuddiesViewController.h
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <AddressBook/AddressBook.h>
#import <AddressBookUI/AddressBookUI.h>


@interface BuddiesViewController : UIViewController<UIActionSheetDelegate>{
    
    UIActionSheet *popupContacts;
    NSMutableArray *arrContacts;
    BOOL is_Searching;
    BOOL let_User_SelectRow;
    PhoneBuddy *objPhnBud;
}

@property(strong,nonatomic)NSMutableArray *arrContactsData;
@property(strong,nonatomic)NSMutableArray *arrContactsCopy;
@property(strong,nonatomic)NSMutableArray *arrBuddiesList;
@end
