//
//  GroupsViewController.h
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface GroupsViewController : UIViewController{
    NSInteger selectedIndex;
}
@property(strong,nonatomic)NSMutableArray *arrGroupsList;
@property(strong,nonatomic)NSMutableArray *arrSavedGroupsList;
@end
