//
//  LocalHelpViewController.h
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface LocalHelpViewController : UIViewController{
    NSString *lat;
    NSString *longi;
    NSMutableArray *arrPoliceStns;
    NSMutableArray *arrHospitals;
}
@property (nonatomic,retain)NSMutableArray *arrAllList;
@end
