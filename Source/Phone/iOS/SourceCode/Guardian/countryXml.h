//
//  countryXml.h
//  Guardian
//
//  Created by PTG on 25/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol countryXmlProtocol <NSObject>

-(void)countryXmlParsedData:(NSMutableArray *)arrData;

@end

@interface countryXml : NSObject <NSXMLParserDelegate>{
    NSMutableDictionary *dict;
    NSMutableArray *arrData;
}
@property (nonatomic , retain) id<countryXmlProtocol> countryXmlDelegate;
+(countryXml *)sharedInstance;
-(void)parseCountryXmlFile ;

@end
