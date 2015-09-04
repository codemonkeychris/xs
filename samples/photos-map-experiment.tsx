/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    var key = "fQVCnK180v01ogsuHZYq~a82hEegA-0yi1_n5Uo7kng~Aj1b4pyjrc02dm0OfXfRTtd0BkZsLdrtxMxBFX6Yqgsq3K3fdEelLTh-WtrIEgxI";

    interface PhotoData {
        color: string;
        geoposition: { latitude: number, longitude: number };
        created: string;
    }

    var constant_photo_data = [
        { color: 'red', geoposition: {latitude:47.55, longitude:-122.35}, created: "9/4/2015" },
        { color: 'orange', geoposition: {latitude:47.57, longitude:-122.32}, created: "9/4/2015" },
        { color: 'purple', geoposition: {latitude:47.58, longitude:-122.31}, created: "9/4/2015" },
        { color: 'green', geoposition: {latitude:47.58, longitude:-122.31}, created: "9/4/2015" },
        { color: 'blue', geoposition: {latitude:47.75, longitude:-122.25}, created: "9/4/2015" },
        { color: 'violet', geoposition: {latitude:47.75, longitude:-122.24}, created: "9/4/2015" }
    ];

    function randomDouble(min, max) {
        return Math.random() * (max - min) + min;
    }
    function randomInt(min, max) {
        return (Math.random() * (max - min) + min) | 0;
    }

    function generateRandomPhotoData(count : number, minLat : number, maxLat: number, minLon:number, maxLon:number) : PhotoData[] {
        var colorChoices = ['red', 'orange', 'yellow'];
        var res :PhotoData[] = [];
        for (var i = 0; i < count; i++) {
            res.push({
                color: colorChoices[randomInt(0,colorChoices.length)],
                geoposition: {latitude: randomDouble(minLat, maxLat), longitude: randomDouble(minLon, maxLon) },
                created: "9/4/2015"
            })
        }
        return res;
    }


    function toRadians(n : number) { return n * Math.PI / 180; };
    function toDegrees(n : number) { return this * 180 / Math.PI; };

    export function haversineDistance(lat1 : number, lon1 : number, lat2 : number, lon2 : number) {
        // from: http://www.movable-type.co.uk/scripts/latlong.html
        var R = 6371000; // metres
        var φ1 = toRadians(lat1);
        var φ2 = toRadians(lat2);
        var Δφ = toRadians(lat2-lat1);
        var Δλ = toRadians(lon2-lon1);

        var a = Math.sin(Δφ/2) * Math.sin(Δφ/2) +
                Math.cos(φ1) * Math.cos(φ2) *
                Math.sin(Δλ/2) * Math.sin(Δλ/2);
        var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));

        var d = R * c;
        return d;
    }

    export function setInitialState() {
        host.setState({ 
            center: "",
            zoomLevel: 7.3,
            centerPoint: {latitude:47.5219, longitude:-122.5875 },
            random_photo_data: generateRandomPhotoData(100, 47.4, 48, -122.50, -122.20)
        });
    };

    function centerChanged(sender) {
        var loc = sender.center.position;

        host.setState({centerPoint: {latitude:loc.latitude, longitude:loc.longitude}});

        host.setState({center: "zoom:" + sender.zoomLevel + ", lat:" + loc.latitude + ", lon:" + loc.longitude });
    }
    function zoomChanged(sender) {
        host.setState({zoomLevel: sender.zoomLevel});
    }

    function renderImage(image, index) {
        return (<Xaml.Grid  map$location={image.geoposition} background={image.color} >
            <Xaml.TextBlock text={'IMG#' + index} />
        </Xaml.Grid>)
    }
    function renderCluster(cluster, index) {
        return (<Xaml.Grid  map$location={{ latitude: cluster.mean[0], longitude: cluster.mean[1] }} background='white' >
            <Xaml.TextBlock text={'C#' + index} />
        </Xaml.Grid>)
    }

    export function render() {
        return renderClusterMap();
    }

    function renderClusterDebug() {
        var data = [
            { t: 'a', d: [1, 2] },
            { t: 'a', d: [2, 1] },
            { t: 'a', d: [10, 11] },
            { t: 'a', d: [12, 13] }
        ]
        
        var r = host.getState().random_photo_data as PhotoData[];
        var clusters1 = DimasKmeans.getClusters(r, { 
            vectorFunction: d=> [d.geoposition.latitude, d.geoposition.longitude],
            distanceFunction: (a, b) => haversineDistance(a[0], a[1], b[0], b[1]),
            numberOfClusters: 20
        });

        for (var i = 0; i < clusters1.length; i++) {
            var c = clusters1[i];
            var mean = c.mean;
            for (var j = 0; j < c.data.length; j++) {
                var item :any = c.data[j];
                item.distance = haversineDistance(mean[0], mean[1], item.geoposition.latitude, item.geoposition.longitude);
            }
        }

        var clusters2 = DimasKmeans.getClusters(data, { 
            vectorFunction: d=> d.d
        });

        var clusters = clusters1;

        return (
            <Xaml.StackPanel>{clusters.map(i=> <Xaml.TextBlock text={JSON.stringify(i) } />)}</Xaml.StackPanel>
        );
    }

    function renderClusterMap() {
        var r = host.getState().random_photo_data as PhotoData[];
        var clusters1 = DimasKmeans.getClusters(r, { 
            vectorFunction: d=> [d.geoposition.latitude, d.geoposition.longitude],
            distanceFunction: (a, b) => haversineDistance(a[0], a[1], b[0], b[1]),
            numberOfClusters: 10,
            maxIterations: 10
        });

        return ( 
            <Xaml.Grid rows={["*", "auto"]}>
                <Xaml.MapControl 
                    zoomLevel={host.getState().zoomLevel} 
                    center={host.getState().centerPoint} 
                    mapServiceToken={key} 
                    onCenterChanged={centerChanged}>
                    <Xaml.MapItemsControl items={clusters1.map(function (c, index) { return renderCluster(c, index); })} />
                </Xaml.MapControl>
                <Xaml.TextBlock fontSize={18} grid$row={1} text={host.getState().center} />
            </Xaml.Grid>
        );
    }

    function renderMap() {
        return ( 
            <Xaml.Grid rows={["*", "auto"]}>
                <Xaml.MapControl 
                    zoomLevel={host.getState().zoomLevel} 
                    center={host.getState().centerPoint} 
                    mapServiceToken={key} 
                    onCenterChanged={centerChanged}>
                    <Xaml.MapItemsControl items={host.getState().random_photo_data.map(function (photo, index) { return renderImage(photo, index); })} />
                </Xaml.MapControl>
                <Xaml.TextBlock fontSize={18} grid$row={1} text={host.getState().center} />
            </Xaml.Grid>
        );
    }
}

module DimasKmeans {
// https://www.npmjs.com/package/dimas-kmeans
    interface DistanceFunction {
        (vector1: number[], vector2: number[]) : number;
    }
    interface VectorFunction<T> {
        (item : T): number[];
    }
    interface GetClustersOptions<T> {
        numberOfClusters?: number, 
        distanceFunction?: DistanceFunction, 
        vectorFunction?: VectorFunction<T>, 
        maxIterations?: number 
    }
    export function getClusters<T>(
        data : T[], 
        options? : GetClustersOptions<T>) : {mean:T, data:T[]}[] {

        var numberOfClusters, distanceFunction, vectorFunction, minMaxValues, maxIterations;

        if (!options || !options.numberOfClusters) { numberOfClusters = getNumberOfClusters(data.length); }
        else { numberOfClusters = options.numberOfClusters; }
        // UNDONE: this limit is required because of the specialization in getClustersWithParams
        //
        numberOfClusters = Math.min(data.length, numberOfClusters);

        if (!options || !options.distanceFunction) { distanceFunction = getDistance; }
        else { distanceFunction = options.distanceFunction; }
        
        if (!options || !options.vectorFunction) { vectorFunction = defaultVectorFunction; }
        else { vectorFunction = options.vectorFunction; }
        
        if (!options || !options.maxIterations) { maxIterations = 1000; }
        else { maxIterations = options.maxIterations; }


        var numberOfDimensions = getNumberOfDimensions(data, vectorFunction);

        minMaxValues = getMinAndMaxValues(data, numberOfDimensions, vectorFunction);

        return getClustersWithParams(data, numberOfDimensions, numberOfClusters, distanceFunction, vectorFunction, minMaxValues, maxIterations).clusters;
    }


    function getClustersWithParams(data, numberOfDimensions ,numberOfClusters, distanceFunction, vectorFunction, minMaxValues, maxIterations) {
        
        var means = createRandomMeans(numberOfDimensions, numberOfClusters, minMaxValues);

        var clusters = createClusters(means);

        var prevMeansDistance = 9999999999;

        var numOfInterations = 0;
        var iterations = [];


        while(numOfInterations < maxIterations) {
            // UNDONE: do we really want this  specialization over general 
            // algorithm, force each group to get something in it
            //
            for (var i = 0; i < numberOfClusters; i++) {
                if (clusters[i].data.length == 0) {
                    for (var j = 0; j < numberOfClusters; j++) {
                        if (clusters[j].data.length > 1) {
                            clusters[i].mean = vectorFunction(clusters[j].data[0]);
                            break;
                        }
                    }
                }
            }

            initClustersData(clusters);

            assignDataToClusters(data, clusters, distanceFunction, vectorFunction);

            updateMeans(clusters, vectorFunction);

            var meansDistance = getMeansDistance(clusters, vectorFunction, distanceFunction);

            //iterations.push(meansDistance);
            // console.log(numOfInterations + ': ' + meansDistance);
            numOfInterations++;
        }
        
        // console.log(getMeansDistance(clusters, vectorFunction, distanceFunction));

        return { clusters: clusters, iterations: iterations };
    }

    function defaultVectorFunction(vector) {
        return vector;
    }

    function getNumberOfDimensions<T>(data:T[], vectorFunction:VectorFunction<T>) : number {
        if (data[0]) {
            return vectorFunction(data[0]).length;
        }  
        return 0;
    }

    function getNumberOfClusters(n) {
        return Math.ceil(Math.sqrt(n/2));
    }

    function getMinAndMaxValues(data, numberOfDimensions, vectorFunction) {

        var minMaxValues = initMinAndMaxValues(numberOfDimensions);

        data.forEach(function (vector) {

            for (var i = 0; i < numberOfDimensions; i++) {
                
                if (vectorFunction(vector)[i] < minMaxValues.minValue[i]) {
                    minMaxValues.minValue[i] = vectorFunction(vector)[i];
                }

                if(vectorFunction(vector)[i] > minMaxValues.maxValue[i]) {
                    minMaxValues.maxValue[i] = vectorFunction(vector)[i];
                }
            };
        });


        return minMaxValues;
    }


    function initMinAndMaxValues(numberOfDimensions) {

        var result = { minValue : [], maxValue : [] }

        for (var i = 0; i < numberOfDimensions; i++) {
            result.minValue.push(9999999999);
            result.maxValue.push(-9999999999);
        };

        return result;
    }


    function getMeansDistance(clusters, vectorFunction, distanceFunction) {

        var meansDistance = 0;

        clusters.forEach(function (cluster) {
            cluster.data.forEach(function (vector) {
                meansDistance = meansDistance + Math.pow(distanceFunction(cluster.mean, vectorFunction(vector)), 2);
            });
        });


        return meansDistance;
    }

    function updateMeans(clusters, vectorFunction) {

        clusters.forEach(function (cluster) {
            updateMean(cluster, vectorFunction);
        });
    }


    function updateMean(cluster, vectorFunction) {
        var newMean = [];

        for (var i = 0; i < cluster.mean.length; i++) {
            newMean.push(getMean(cluster.data, i, vectorFunction));
        };

        cluster.mean = newMean;
    }

    function getMean(data, index, vectorFunction) {
        var sum =  0;
        var total = data.length;

        if(total == 0) return 0;

        data.forEach(function (vector) {
            sum = sum + vectorFunction(vector)[index];
        });

        return sum / total;
    }

    function assignDataToClusters(data, clusters, distanceFunction, vectorFunction) {


        data.forEach(function (vector) {
            var cluster = findClosestCluster(vectorFunction(vector), clusters, distanceFunction);

            if(!cluster.data) cluster.data = [];
            
            cluster.data.push(vector);
        });
    }


    function findClosestCluster(vector, clusters, distanceFunction) : any {

        var closest = {};
        var minDistance = 9999999999;

        clusters.forEach(function (cluster) {
            var distance = distanceFunction(cluster.mean, vector);
            if (distance < minDistance) {
                minDistance = distance;
                closest = cluster;
            };
        });

        return closest;
    }

    function initClustersData(clusters) {
        clusters.forEach(function (cluster) {
            cluster.data = [];
        });
    }

    function createClusters(means) {
        var clusters = [];

        means.forEach(function (mean) {
            var cluster = { mean : mean, data : []};

            clusters.push(cluster);
        });

        return clusters;
    }
 
    function createRandomMeans(numberOfDimensions, numberOfClusters, minMaxValues) {
        
        var means = [];

        for (var i = 0; i < numberOfClusters; i++) {
            means.push(createRandomPoint(numberOfDimensions, minMaxValues.minValue, minMaxValues.maxValue));
        };

        return means;
    }

    function createRandomPoint(numberOfDimensions:number, minValues:number[], maxValues:number[]) {
        var point = [];

        for (var i = 0; i < numberOfDimensions; i++) {
            point.push(random(minValues[i], maxValues[i]));
        };
        
        return point;
    }

    function random (low, high) {
        return Math.random() * (high - low) + low;
    }

    function getDistance(vector1, vector2) {
        var sum = 0;

        for (var i = 0; i < vector1.length; i++) {
            sum = sum + Math.pow(vector1[i] - vector2[i],2)
        };

        return Math.sqrt(sum);
    }
}