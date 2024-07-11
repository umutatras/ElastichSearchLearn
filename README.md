Elasticsearch|Architecture	

Document: Kaydetmek istediğimiz datalar document olarak adlandırılır. Elasticsearch dataları json olarak tutar ve kendi data tiplerine dönüştürür

Index: Birbiriyle ilişkili verileri indexler.Örn products indexleri products ile ilgili verileri indexlemedir.

DataStreams: Zaman temelli data demektir. İndexleri tarihlere göre tutma yeri. Performans açısından indexleri bölme,bulma işlemlerini kolaylaştırır.

Shard: Dataların ayrıldığı yer nereye kaydedileceği yeri ayarlar(Hash algoritması kullanılır).Sharda gelen datalar segment denen kısımlara kaydedilir.

Node: Sanal makinedir.

Cluster: Node'ların ait olduğu yapı

Replica: Dataların kopyasıdır.
 
Text Analyzing: Veri geldiğinde bir analiz sürecinden geçer bunada text analyzing deniliyor. 2 aşamadan geçiyor tokenization ve normalization.

Tokenization: Burada gelen text boşluklar ile parçalara ayrılır

Normalization: Aşamasında datanın eş anlamlı veya kökü ile kaydedilir.

Bu işlemler sonucunda inverted index kısmına eklenir.

Inverted Index: Örnek olarak bir kelime geldiğinde hangi sayfada olduğunun tutulduğu kısımdır. Word: Hello, Doc Number:1,5,7 benzeri. Burada filtreleme işlemleri de yapılıyor.Örn: html taglarının temizlenmesi gibi.

Relevancy: Aranan kelime ile dönen datada en alakalı sonuçlar ilişkisini temsil eder ve skor sıralaması vardır. Elastichsearch skor değeri en yüksek veriyi en başa alır. Dikkat etmemiz gereken şey ise tarihe göre sıralama yapmamalıyız skor değerini kaybedebiliriz.


get products/_doc/1: 1 id'li datayı getirir.

put products/_doc/1
{
	"name":"nokia",
	"rating":"10"
}

bu kod bloğu ise 1 numaralı veri var ise güncelle yok ise ekler.

put product_create/1
{
	"name":"nokia",
	"rating":"10"
}
bu kod bloğu ise daha önce bu veri eklenmiş mi diye kontrol eder. eğer eklenmiş ise hata fırlatır. eklenmemişse veriyi kaydeder.


POST products/_doc
{
	"name":"nokia",
	"rating":"10"
}

elasticsearch kendi tarafında id üretip kaydeder. bunu post ile yapabiliyoruz.


İndexleme nasıl meydana geliyor?

Elk gönderdiğimiz datayı memory buffer'a alır. Bu data daha sonra shard'lara gelir ve segmentlere dağıtılır. buradan ise file systeme aktarılır.

PUT products/_settings
{
  "index":{"refresh_interval":"5s"}
}

bu kod bloğunda ise verinin ne kadar zaman sonra sorgulanabileceği belirtiliyor.

PUT products/_doc/20?refresh=wait_for
{
  "name":"mühendis"
}

bu kod bloğu ise verinin anlık olarak sorgulanıp sorgulanmaması parametresinin belirtiği yerdir.Default false'tur.True ise sorgulanır wait for ise bekleniyor demektir. Default 1 sndir.


get products/_doc/1

post products/_update/1
{
  "doc":{
    "name":"nokia111",
    "rating":9
  }
}

güncellemenin farklı bir yolu. Güncellemeye çalıştığımız veri yoksa hata verir.


DELETE products/_doc/1
veriyi silmek için kullandığımız kod.

HEAD products/_doc/12
datanın var olup olmadığını kontrol eder sadece 404 veya 200 döner.

GET products/_search
{
  "query": {"match_all": {}}
}

eşleşen tüm dataları getirir.

GET products/_mget
{
  "ids": [1,3]
}

belirttiğimiz idlerin datasını getirir.

get products/_mget
{
  "docs":[
    {
        "_index":"products",
    "_id":1
    }
  ]
}
mget farklı bir kullanımı

get products/_source/1

sadece source kısmını alır 

get products/_doc/1?_source_includes=name
bu sorguda ise includes ile sadece name kısmı getirilir. excludes yapıldığında ise harici bırakır.


Data types

Common types 
-text (string values)
-boolean (true,false)
-date (2024-05-20)
-numeric(int long byte float double)
-binary

Complex and relational types
-object (Json object)(tutacağımız datalar tipleriyle birlikte belli ise object olarak tutulur.)
-nested (objectte yapabildiğimiz her şeyi burada yapabiliyoruz.aramada farklılık var.Propertylerde verilen şartlara göre eleme yapar birbiriyle ilişkilendirir.Maliyetlidir.)
-flattened (Kaydedeceğimiz datanın property sayısını bilmiyorsak bunu kullanabiliriz.her bir datayı keyword olarak alır.)
-join (İlişkisel veriler olarak düşünebiliriz.)

Structured types
-geo_shape
-geo_point
-ip
-date_range(05/05/2024-20/05/2024)
-ip_range


Mapping

Schema (kaydedilmek istenen data tipleri) oluşturma süreci mapping olarak adlandırılır. Tipleri belirlemezsek elk kendisi belirler.
Tipleri elk bırakmamak best practice'dir.

Mapping Types

Dynamic mappings:Biz bir tip belirtmesi yapmazsak bunu elk yapar.
Explicit mappings:Dynamicin tersi.

get pens/_mapping

pens'lerin hangi tipte kaydedildiğini getirir.

put cars/_mapping
{

    "properties": {
      "name":{
        "type": "text",
        "fields":{
          "kwd":{
            "type":"keyword"
          }
        }
      }
      }
      }
bu kod bloğunda name için 2 tane tip tanımlaması yapılabilir.

Re-indexing
Sonradan eklenen bir prop için ekstra tip için diğer veriler içinde güncellenmesi için yapılan işlemdir.


post /_reindex
{
"source":{
"index":"cars"
}
"dest":{
"index":"cars2"
}
}

Search Types

Structured Types
-date between
-rating
-price
yani value type değerlerde arama yapılmak istenirse bu veri tipleri structured kısmına giriyor.

Unstructured Types(fulltext)
-keyword olarak tanımladığımız datalarda arama yapılmak istenirse buda fulltext kısmına giriyor.Burada skor değeri vardır.Skor değeri en ilişkili verileri temsil eder.


Searching Rest API

Query DSL: Bir istek yaparken arama ile ilgili paremetleri requesting bodysinde gönderiyoruz.

örnek olarak:
Get kibana_sample_data/_search{
"query":{
"match":{
"user":yahya}
}}


Uri request searching example:
get kibana_sample_data/_search?q=user:mary
user'ı maryy olanı getir

get kibana_samp_data/_search?q=customername:umut and customerlastname:atras
adı umut soyadı atras olanı getir


Term-level query nedir?

Structured Types'larda yapmış olduğumuz aramaları belirtiyor.
Kesin bir değer üzerinde arama yapılır(tarih aralığı,fiyat aralığı veya idsi üzerinden arama vb.)

örnek:
GET cars/_search
{
  "query":{
    "term":{
      "price":{
        "value":10
      }
    }
  }
  }

POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "term": {
      "customer_first_name.keyword": {
        "value": "sonya",
        "case_insensitive":true
      }
    }
  }
}
"case_insensitive":büyük küçük harf duyarlılığını kaldırır.

POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "terms": {
      "customer_id":  ["45","38","14"]
      
    }
  }
}
Birden fazla değere göre veri çekmek istediğimizde çoğul olan terms'i kullanıyoruz.

POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "ids": {
      "values": ["KWdsfI8B6dJlNyeBBRWQ","LWdsfI8B6dJlNyeBBRWQ"]
      
    }
  }
}

dataların idleri meta tarafında bulunduğu için ids özel sorgulama yapıyoruz.

Exist Query

Bir verinin var olup olmadığını kontrol etmek istenildiği zaman kullanılır.

POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "exists": {
      "field": "user"
      
    }
  }
}
user adında fieldlar varsa veriyi döner.

Prefix Query

Belli bir karakter ile başlayan verileri getirir.(Dotnetteki startwh komutunu eş değeridir)

POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "prefix": {
      "customer_first_name.keyword": {
        "value": "Edd"
      }
    }
  }
}

Edd ile başlayanları getirir.

Range Query

Belli bir aralığa düşen dataları getirir.

POST kibana_sample_data_ecommerce/_search
{
  "query": {
   "range": {
     "totalPrice": {
       "gte": 10,
       "lte": 20
     }
   }
    }
  }
toplam fiyatı 10dan büyük 20den küçük olanları getirir.

Wildcard Query
Programlamada like eşdeğeridir.

POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "wildcard": {
      "customer_first_name.keyword": {
        "value": "*Edd*"
      }
    } 
   }
    }

Fuzzy Query
Kullanıcı arama yaptığı zaman harf hatası yaptığında elk bunu göz ardı ederek arama yapıyor.

POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "fuzzy": {
      "customer_first_name.keyword": {
       "value": "Stephani",
       "fuzziness": 1
      }
      
    }
  
   }
    }    
    
 "fuzziness": bu değer kaç tane harf hatası kabul edilebileceğini belirtir.

Pagination

POST kibana_sample_data_ecommerce/_search
{
  "size": 20,
  "from": 0, 
  "query": {
    "fuzzy": {
      "customer_first_name.keyword": {
       "value": "Stephani",
       "fuzziness": 1
      }
      
    }
  
   }
    }   

size:kayıt getirme adedi
from:kaç tanesini hariç tutacak(ilk değerden itibaren)

Response(includes,excludes)
Select sorgusu benzeridir.
    POST kibana_sample_data_ecommerce/_search
{
  "_source": {
    "includes": ["category","customer_first_name","currency"],
    "excludes":  ["category","customer_first_name","currency"]
  }, 
  "size": 20,
  "from": 0, 
  "query": {
    "fuzzy": {
      "customer_first_name.keyword": {
       "value": "Stephani",
       "fuzziness": 1
      }
      
    }
  
   }
    
Sort Query
Datayı sıralama işlemi. Fulltext search'te bunu kullanmak değerlerin alaka seviyesi ile ilgili problem yaratabilir. Term querylerle kullanmak best practice'dir.
POST kibana_sample_data_ecommerce/_search
{
  "size": 20, 
  "query": {
    "term": {
      "customer_gender": {
        "value": "MALE"
      }
    }
  },
  "sort": [
    {
      "taxful_total_price": {
        "order": "desc"
      }
    }
  ]
}

Match Query
Arama yapmak istediğimiz fulltextsearch için kullandığımız parametre.
POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "match": {
      "category": "Women's"
    }
  }
}

POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "match": {
      "customer_full_name":{
        "query": "yahya goodwin",
        "operator": "and"
      }
    }
  }
}

İki kodun birbirinden farkı üst kısımdaki arama yaparken or alttaki ise and ile yapar.


Multi match Query
Birde fazla alanda arama yapabilmemizi sağlar.
POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "multi_match": {
      "query": "Sultan",
      "fields": ["customer_first_name","customer_full_name","customer_last_name"]
    }
  }
}

Match Bool Prefix
Bu örnekte son kısımdaki moran ile başlayacak geri kalan kısmında ne var ise getiren bir komuttur.
POST kibana_sample_data_ecommerce/_search
{
  "query": {
    "match_bool_prefix":{
      "customer_full_name":"Sultan Al Moran"
    }
    }
  }

Match Phrase Query
İstenen verinin sıralamasının aynısına göre arama yapar. Örnek olarak sultan 1. sırada al 2. sırada moran 3. sırada. Birebir eşleme

 POST kibana_sample_data_ecommerce/_search
  {
    "query": {
      "match_phrase": {
        "customer_full_name": "Sultan Al Moran"
      }
    }
  }
Match Phrase Prefix
Sultan Al Moran içeren ve sıralama olarak aynı olan kısımlar için kullanılır.
  POST kibana_sample_data_ecommerce/_search
  {
    "query": {
      "match_phrase_prefix": {
        "customer_full_name": "Sultan Al Moran"
      }
    }
  }  

Compound Query
Must: bir sorgu yazarken kullanıyorsak kesin olarak o datanın bulunması gerekiyor ve skor kısmına katkı sağlıyor.
Filter: Must'ın aynısı fakat skora katkı sağlamaz.
Should: Or gibi davranır. zorunlu değildir. eğer aranan kelime text var ise skora katkı sağlar.
Must not: İstemediğimiz şartlar var ise bunu kullanıyoruz ve skora katkı sağlamaz.
GET kibana_sample_data_ecommerce/_search
{
  "query": {
    "bool": {
      "must": [
        {
          "term": {
            "geoip.city_name": {
              "value": "New York"
            }
          }
        }
      ],
      "must_not": [
        {
          "range": {
            "taxful_total_price": {
              "lte": 100
            }
          }
        }
      ],
      "should": [
        {
          "term": {
            "category.keyword": {
              "value": "Women's Clothing"
            }
          }
        }
      ],
      "filter": [
        {
          "term": {
            "manufacturer.keyword": "Tigress Enterprises"
          }
        }
      ]
    }
  }
}








