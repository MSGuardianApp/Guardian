//
//  MyGroupsViewController.h
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <MessageUI/MessageUI.h>


@interface MyGroupsViewController : UIViewController<MFMessageComposeViewControllerDelegate,MFMailComposeViewControllerDelegate>
@property (nonatomic ,retain)NSMutableArray *arrGroups;
@end
