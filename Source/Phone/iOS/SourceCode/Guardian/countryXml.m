//
//  countryXml.m
//  Guardian
//
//  Created by PTG on 25/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "countryXml.h"

@implementation countryXml

static countryXml *sharedInstance;
+(countryXml *)sharedInstance
{
	{
		if (!sharedInstance)
			sharedInstance = [[countryXml alloc] init];
		return sharedInstance;
	}
}


-(void)parseCountryXmlFile {
    if(arrData.count > 0){
        [self xmlParsingFinished];
    }
    else{
        arrData = [[NSMutableArray alloc] init];
        
        
        NSString * filePath = [[NSBundle mainBundle] pathForResource:@"country_codes" ofType:@"xml"];
        
        NSURL * fileURL = [NSURL fileURLWithPath:filePath];
        NSXMLParser * xmlParser = [[NSXMLParser alloc] initWithContentsOfURL:fileURL];
        
        [xmlParser setDelegate:self];
        [xmlParser setShouldResolveExternalEntities:YES];
        [xmlParser parse];
    }
    
}


#pragma mark -
#pragma mark NSXMLParser delegates

-(void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName
 namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName
   attributes:(NSDictionary *)attributeDict
{
    if ([elementName isEqualToString:@"Country"]) // This is the only tag i was received from my webservice response.
    {
        dict = [[NSMutableDictionary alloc] init];
        
        [dict setObject:[attributeDict objectForKey:@"Name"] forKey:@"Name"];
        [dict setObject:[attributeDict objectForKey:@"IsdCode"] forKey:@"IsdCode"];
    }
    else if ([elementName isEqualToString:@"Properties"]){
        [dict setObject:[attributeDict objectForKey:@"MaxPhoneDigits"] forKey:@"MaxPhoneDigits"];
        [dict setObject:[attributeDict objectForKey:@"Police"] forKey:@"Police"];
        [dict setObject:[attributeDict objectForKey:@"Ambulance"] forKey:@"Ambulance"];
        [dict setObject:[attributeDict objectForKey:@"Fire"] forKey:@"Fire"];
    }
}

-(void)parser:(NSXMLParser *)parser foundCharacters:(NSString *)string
{
}

-(void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
 namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName
{
    if ([elementName isEqualToString:@"Country"])
    {
        [arrData addObject:dict];
    }
}
- (void)parserDidEndDocument:(NSXMLParser *)parser{
    NSLog(@"%@",arrData);
    [self xmlParsingFinished];
}
- (void)parser:(NSXMLParser *)parser parseErrorOccurred:(NSError *)parseError{
    
}
-(void) xmlParsingFinished
{
    [self.countryXmlDelegate countryXmlParsedData:arrData];
}

@end
