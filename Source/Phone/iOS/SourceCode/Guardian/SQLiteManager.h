//
//  SQLiteManager.h
//  Massy Card
//
//  Created by PTG on 13/08/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "sqlite3.h"

enum errorCodes {
    kDBNotExists,
    kDBFailAtOpen,
    kDBFailAtCreate,
    kDBErrorQuery,
    kDBFailAtClose
};

@interface SQLiteManager : NSObject {
    NSString *lastID;
    sqlite3 *db; // The SQLite db reference
    NSString *databaseName; // The database name
}



- (id)initWithDatabaseNamed:(NSString *)name;

// SQLite Operations
- (NSError *) openDatabase;
- (NSError *) doQuery:(NSString *)sql;
- (NSArray *) getRowsForQuery:(NSString *)sql;
- (NSError *) closeDatabase;
- (NSString *) getlastInsertedId;
-(BOOL)recordExistOrNot:(NSString *)query;
- (NSString *)getDatabaseDump;

@end
