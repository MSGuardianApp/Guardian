//
//  AboutHostViewController.h
//  Guardian
//
//  Created by PTG on 10/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "ViewPagerController.h"
#import <MessageUI/MessageUI.h>
@interface AboutHostViewController : ViewPagerController<ViewPagerDelegate,ViewPagerDataSource,MFMailComposeViewControllerDelegate,MFMessageComposeViewControllerDelegate>{
    NSMutableArray *arrContent;

}

@end
