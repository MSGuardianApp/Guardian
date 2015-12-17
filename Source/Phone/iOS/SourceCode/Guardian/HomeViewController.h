//
//  HomeViewController.h
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <CoreLocation/CoreLocation.h>
#import <MessageUI/MessageUI.h>

@interface HomeViewController : UIViewController<UIActionSheetDelegate,MFMessageComposeViewControllerDelegate>{
    UIActionSheet *popup;
}
-(void)countSOSTrackupdation:(NSArray *)arr;
-(void)CurrentLocationIdentifier;
@end
