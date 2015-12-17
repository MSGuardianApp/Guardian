//
//  TrackMeViewController.h
//  Guardian
//
//  Created by PTG on 02/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <MapKit/MapKit.h>
#import "AnnoPin.h"
//#import "BingMaps/BingMaps.h"
//BMMapViewDelegate
#import <MessageUI/MessageUI.h>

@interface TrackMeViewController : UIViewController<MFMessageComposeViewControllerDelegate,UIWebViewDelegate,UIImagePickerControllerDelegate,UINavigationControllerDelegate> {
//    BMMapView *mapView ;
    IBOutlet  UIWebView *webV;
    GeoTag *loc;
    NSMutableArray *arrRoutePoints;
    NSMutableArray *searchPoints;
    CLLocationCoordinate2D coordinateUpto;
    MKPolyline *polyline;
    BOOL isRouteEnabled;
    BOOL destValueAssigned;
    BOOL isTrackingRoute;
    AnnoPin *objLastAnnote;
    MKRoute *routeDetails;
    
}
@property (nonatomic,retain) MKPolyline *polyline;
@property (nonatomic,strong) UILongPressGestureRecognizer *lpgr;
@property (strong, nonatomic) NSString *allSteps;
@end
