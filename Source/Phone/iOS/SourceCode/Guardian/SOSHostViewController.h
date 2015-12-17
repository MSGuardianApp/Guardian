//
//  SOSHostViewController.h
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "ViewPagerController.h"
#import <MessageUI/MessageUI.h>

@interface SOSHostViewController : ViewPagerController<UIImagePickerControllerDelegate,UINavigationControllerDelegate,LocationPostProtocol,MFMessageComposeViewControllerDelegate>
{
    NSMutableArray *arrContent;
    NSTimer *timer;
    NSInteger counter;
    BOOL startPushpin;
    GlobalClass *objGlobalClass;
    NSMutableArray *arrPhone ;
    NSMutableArray *arrBuddies;
    NSString *msg;
}
@end
