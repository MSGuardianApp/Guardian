package com.guardianapp.database;

import com.guardianapp.utilities.LogUtils;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;

public class DatabaseHandler
{
	private SQLiteDatabase db;
	private Context context;
	private MySQlLiteOpenHelper dbHelper;
	private static final String DATABASE_NAME = "guardian.db";
	private static final int DATABASE_VERSION = 1;

	public DatabaseHandler(Context c) {
		// TODO Auto-generated constructor stub
		this.context = c;
		dbHelper = new MySQlLiteOpenHelper(context);
	}

	class MySQlLiteOpenHelper extends SQLiteOpenHelper {
		public MySQlLiteOpenHelper(Context context) {
			super(context, DATABASE_NAME, null, DATABASE_VERSION);
		}

		@Override
		public void onCreate(SQLiteDatabase database) {
			try {
				LogUtils.LOGD(DBQueries.TABLE_BUDDY,DBQueries.CREATE_TABLE_BUDDY);
				database.execSQL(DBQueries.CREATE_TABLE_BUDDY);

				LogUtils.LOGD(DBQueries.TABLE_GROUP,DBQueries.CREATE_TABLE_GROUPS);
				database.execSQL(DBQueries.CREATE_TABLE_GROUPS);

				LogUtils.LOGD(DBQueries.TABLE_HEALTH,DBQueries.CREATE_TABLE_HEALTH);
				database.execSQL(DBQueries.CREATE_TABLE_HEALTH);

				LogUtils.LOGD(DBQueries.TABLE_LOCATION_BUDDY,DBQueries.CREATE_TABLE_HEALTH);
				database.execSQL(DBQueries.CREATE_TABLE_LOCATION_BUDDY);

				LogUtils.LOGD(DBQueries.TABLE_PROFILE,DBQueries.CREATE_TABLE_PROFILE);
				database.execSQL(DBQueries.CREATE_TABLE_PROFILE);

				LogUtils.LOGD(DBQueries.TABLE_USER,DBQueries.CREATE_TABLE_USER);
				database.execSQL(DBQueries.CREATE_TABLE_USER);

			} catch (Exception e) {
				LogUtils.LOGE("DB ERROR","Table is not Crete");
			}


		}

		@Override
		public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
			LogUtils.LOGE(DatabaseHandler.class.getName(),
					"Upgrading database from version " + oldVersion + " to "
							+ newVersion + ", which will destroy all old data");
			db.execSQL("DROP TABLE IF EXISTS " + DBQueries.TABLE_BUDDY);
			db.execSQL("DROP TABLE IF EXISTS " + DBQueries.TABLE_GROUP);
			db.execSQL("DROP TABLE IF EXISTS " + DBQueries.TABLE_HEALTH);
			db.execSQL("DROP TABLE IF EXISTS " + DBQueries.TABLE_LOCATION_BUDDY);
			db.execSQL("DROP TABLE IF EXISTS " + DBQueries.TABLE_PROFILE);
			db.execSQL("DROP TABLE IF EXISTS " + DBQueries.TABLE_USER);

			onCreate(db);
		}


	} 


	public DatabaseHandler openDatabase(){
		db = dbHelper.getWritableDatabase();
		return this;
	}

	public void closeDatabase(){
		dbHelper.close();
	}


	public long insertRecord(String tableName, ContentValues con){
		return db.insert(tableName, null, con);
	}

	public Cursor getAllRecords(String tableName){

		return db.query(tableName, null, null, null, null, null, null); 
	}

	public Cursor selectRecords(String query){

		return db.rawQuery(query, null); 
	}

	public void deleteAllRecords(String tableName){
		db.delete(tableName, null, null);
	}

	void deleteOneRecord(String tableName,String rowid,String colum ){

		db.delete(tableName, rowid+"= '" + colum + "'", null);
	}

	void updateRecord(String tableName,ContentValues value,String whereClause,String[] whereArgs ){

		db.update(tableName, value, whereClause, whereArgs);
	}

}