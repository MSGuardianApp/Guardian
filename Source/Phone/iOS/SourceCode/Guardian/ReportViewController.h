//
//  ReportViewController.h
//  Guardian
//
//  Created by PTG on 19/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface ReportViewController : UIViewController <UIImagePickerControllerDelegate,UINavigationControllerDelegate>{
    NSString *lat;
    NSString *longi;
    NSString *incidentStr;
}
@end
