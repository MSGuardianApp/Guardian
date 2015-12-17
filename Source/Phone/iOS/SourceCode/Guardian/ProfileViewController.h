//
//  ProfileViewController.h
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "HostViewController.h"
@interface ProfileViewController : UIViewController <NSURLConnectionDelegate>{
    BOOL isLiveConnectOpened;
    HostViewController *hostVC;
}

@end
