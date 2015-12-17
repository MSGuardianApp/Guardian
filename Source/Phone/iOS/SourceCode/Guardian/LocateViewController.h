//
//  LocateViewController.h
//  Guardian
//
//  Created by PTG on 22/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <MapKit/MapKit.h>

@interface LocateViewController : UIViewController<MFMessageComposeViewControllerDelegate,MFMailComposeViewControllerDelegate>{
    NSMutableArray *arrList;
    NSTimer *timer;
}

@end
