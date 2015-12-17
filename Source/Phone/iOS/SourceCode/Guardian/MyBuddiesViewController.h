//
//  MyBuddiesViewController.h
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <MessageUI/MessageUI.h>

@interface MyBuddiesViewController : UIViewController <MFMessageComposeViewControllerDelegate,MFMailComposeViewControllerDelegate>{
    NSString *msg ;
}
@property (nonatomic ,retain)NSMutableArray *arrBuddies;
@end
