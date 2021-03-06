﻿using System.Collections.Generic;
using Mongo.Entity.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Mongo.Entity.Extensions {

	public static class bson_document_extensions {
		/// <summary>
		/// Creates a UpdateDocument only containing the entity’s properties that are not Null
		/// </summary>
		public static UpdateDocument as_update_document( this IMongoEntity entity ) {
			return as_update_document(entity, true);
		}

		/// <summary>
		/// Creates a UpdateDocument specifying whether or not the entity’s Null properties should be included or not
		/// </summary>
		public static UpdateDocument as_update_document( this IMongoEntity entity, bool exclude_null_values ) {
			var bson_document = entity.ToBsonDocument();

			// TODO: consider not removing this elements here, instead add an extension method on NODUS.MongoDB project that does such thing
			bson_document.Remove( "_id" );
			bson_document.Remove( "_t" );

			if( exclude_null_values ) {
				var to_remove = new List<BsonElement>( );

				foreach( var item in bson_document.Elements ) {
					if( item.Value.IsBsonNull ) to_remove.Add( item );
				}

				foreach( var item in to_remove ) {
					bson_document.RemoveElement( item );
				}
			}

		 	return new UpdateDocument( "$set", bson_document );
		}
	}

}