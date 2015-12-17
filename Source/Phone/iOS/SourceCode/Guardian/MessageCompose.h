//
//  MessageCompose.h
//  Guardian
//
//  Created by PTG on 10/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MessageUI/MessageUI.h>

@interface MessageCompose : NSObject <MFMessageComposeViewControllerDelegate>
@property (nonatomic , retain) UIViewController *fromVC;
- (void)showSMS:(NSString*)message ForRecipents:(NSArray *)recipents;
@end
