/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    var key = "fQVCnK180v01ogsuHZYq~a82hEegA-0yi1_n5Uo7kng~Aj1b4pyjrc02dm0OfXfRTtd0BkZsLdrtxMxBFX6Yqgsq3K3fdEelLTh-WtrIEgxI";

    var displayModes = ["flat images", "dbscan clusters", "kmeans clusters", "debug clusters", "n/a"];

    export function setInitialState() {
        host.setState({ 
            center: "",
            mpp: 84,
            zoomLevel: 10.3,
            activeDisplayMode: displayModes[0],
            centerPoint: {latitude:47.5219, longitude:-122.5875 },
            random_photo_data: generateRandomPhotoData(100, 47.4, 48, -122.50, -122.20)
        });
    };

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

    function groundResolution(lat:number, zoomLevel:number) {
        // https://msdn.microsoft.com/en-us/library/bb259689.aspx
        return (Math.cos(toRadians(lat)) * 2 * Math.PI * 6378137) / (256 * Math.pow(2,zoomLevel));
    }

    function centerChanged(sender) {
        var loc = sender.center.position;

        host.setState({centerPoint: {latitude:loc.latitude, longitude:loc.longitude}});
        host.setState({ mpp: groundResolution(loc.latitude, sender.zoomLevel) });
        host.setState({center: "zoom:" + sender.zoomLevel + ", lat:" + loc.latitude + ", lon:" + loc.longitude + ", mpp:" + groundResolution(loc.latitude, sender.zoomLevel) });
    }
    function zoomChanged(sender) {
        host.setState({zoomLevel: sender.zoomLevel});
    }

    function renderImage(image, index) {
        return (<Xaml.Grid opacity={0.5} map$location={image.geoposition} background={image.color} >
            <Xaml.TextBlock text={'IMG#' + index} />
        </Xaml.Grid>)
    }

    function renderCluster(cluster, index) {
        return (<Xaml.Grid  map$location={{ latitude: cluster.mean[0], longitude: cluster.mean[1] }} background='white' >
            <Xaml.TextBlock text={'C#' + index} />
        </Xaml.Grid>)
    }

    function modeChanged(sender, e) {
        host.setState({ activeDisplayMode: xsrt.getSelectedItem(sender) });
    }

    export function render() {
        switch (host.getState().activeDisplayMode) {
            case "flat images":
                return renderMap();
            case "dbscan clusters":
                return renderClusterMap(host.getState().activeDisplayMode);
            case "kmeans clusters":
                return renderClusterMap(host.getState().activeDisplayMode);
            case "debug clusters":
                return renderClusterDebug();
            default:
            return (
                <Xaml.StackPanel>
                    <Xaml.StackPanel orientation='Horizontal'>
                        <Xaml.TextBlock text='select mode:' />
                        <Xaml.ComboBox 
                            onSelectionChanged={modeChanged}
                            selectedItem={host.getState().activeDisplayMode} 
                            itemsSource={displayModes} />
                    </Xaml.StackPanel>
                    <Xaml.TextBlock text={"unexpected mode: '" + host.getState().activeDisplayMode + "'"} />
                </Xaml.StackPanel>
            );
        }
    }

    class ConfigBar {
        props: Xaml.StackPanelProps; 
        render() { 
            return (
                <Xaml.StackPanel orientation='Horizontal'>
                    <Xaml.TextBlock text='select mode:' />
                    <Xaml.ComboBox 
                        onSelectionChanged={modeChanged}
                        selectedItem={host.getState().activeDisplayMode} 
                        itemsSource={displayModes} />
                </Xaml.StackPanel>
            );
        };
    }

    function renderClusterDebug() {
        var r = host.getState().random_photo_data as PhotoData[];
        var clusters1 = DistanceThreshold.getClusters(r, { 
            vectorFunction: d=> [d.geoposition.latitude, d.geoposition.longitude],
            distanceFunction: (a, b) => haversineDistance(a[0], a[1], b[0], b[1]),
            desiredDistance: host.getState().mpp * 41,
            idealGroupSize: 2
        });

        var clusters2 = DimasKmeans.getClusters(r, { 
            vectorFunction: d=> [d.geoposition.latitude, d.geoposition.longitude],
            distanceFunction: (a, b) => haversineDistance(a[0], a[1], b[0], b[1]),
            numberOfClusters: 20,
            maxIterations: 10
        });

        var clusters = clusters1;

        for (var i = 0; i < clusters.length; i++) {
            var c = clusters[i];
            c.data = null;
            /*
            var mean = c.mean;
            for (var j = 0; j < c.data.length; j++) {
                var item :any = c.data[j];
                item.distance = haversineDistance(mean[0], mean[1], item.geoposition.latitude, item.geoposition.longitude);
            }
            */
        }

        return (
            <Xaml.StackPanel>
                <ConfigBar />
                <Xaml.StackPanel>{clusters.sort((a, b) => a.maxDistance - b.maxDistance).map(i=> <Xaml.TextBlock text={JSON.stringify(i) } />) }</Xaml.StackPanel>
            </Xaml.StackPanel>
        );
    }

    function renderClusterMap(approach:string) {
        var r = host.getState().random_photo_data as PhotoData[];
        var clusters1 = DistanceThreshold.getClusters(r, { 
            vectorFunction: d=> [d.geoposition.latitude, d.geoposition.longitude],
            distanceFunction: (a, b) => haversineDistance(a[0], a[1], b[0], b[1]),
            desiredDistance: host.getState().mpp * 35,
            idealGroupSize: 2
        });

        var clusters2 = DimasKmeans.getClusters(r, { 
            vectorFunction: d=> [d.geoposition.latitude, d.geoposition.longitude],
            distanceFunction: (a, b) => haversineDistance(a[0], a[1], b[0], b[1]),
            numberOfClusters: 20,
            maxIterations: 10
        });

        var clusters;

        switch (approach) {
            case "dbscan clusters":
                clusters = clusters1;
                break;
            case "kmeans clusters":
                clusters = clusters2;
                break;
            default:
                throw "whoops!";    
        }

        var renderedClusters = clusters.map(function(c, index) { return renderCluster(c, index); });
        var renderedImages = r.map(function(photo, index) { return renderImage(photo, index); });

        var all = renderedImages.concat(renderedClusters);

        return ( 
            <Xaml.Grid rows={["auto", "*", "auto"]}>
                <ConfigBar grid$row={0} />
                <Xaml.MapControl 
                    grid$row={1} 
                    zoomLevel={host.getState().zoomLevel} 
                    center={host.getState().centerPoint} 
                    mapServiceToken={key} 
                    onCenterChanged={centerChanged}
                    onZoomLevelChanged={centerChanged}
                    >
                    <Xaml.MapItemsControl items={all} />
                </Xaml.MapControl>
                <Xaml.TextBlock fontSize={18} grid$row={2} text={host.getState().center} />
            </Xaml.Grid>
        );
    }

    function renderMap() {
        return ( 
            <Xaml.Grid rows={["auto", "*", "auto"]}>
                <ConfigBar grid$row={0} />
                <Xaml.MapControl 
                    grid$row={1}
                    zoomLevel={host.getState().zoomLevel} 
                    center={host.getState().centerPoint} 
                    mapServiceToken={key} 
                    onCenterChanged={centerChanged}
                    onZoomLevelChanged={centerChanged}
                    >
                    <Xaml.MapItemsControl items={host.getState().random_photo_data.map(function (photo, index) { return renderImage(photo, index); })} />
                </Xaml.MapControl>
                <Xaml.TextBlock fontSize={18} grid$row={2} text={host.getState().center} />
            </Xaml.Grid>
        );
    }
}

module DistanceThreshold {
    interface DistanceFunction {
        (vector1: number[], vector2: number[]) : number;
    }
    interface VectorFunction<T> {
        (item : T): number[];
    }
    interface GetClustersOptions<T> {
        numberOfClusters?: number;
        distanceFunction?: DistanceFunction;
        vectorFunction?: VectorFunction<T>;
        desiredDistance?: number;
        idealGroupSize?: number;
    }
    interface Cluster<T> {
        mean: number[];
        data: T[];
        maxDistance?: number;
        count?: number;
    }
    export function getClusters<T>(
        data: T[],
        options?: GetClustersOptions<T>): Cluster<T>[] {

        options = options || {};
        options.distanceFunction = options.distanceFunction || defaultDistance;
        options.vectorFunction = options.vectorFunction || defaultVector;
        options.desiredDistance = options.desiredDistance || 10;
        options.idealGroupSize = options.idealGroupSize || 2;

        var clusters = createClusters(data, options.vectorFunction(data[0]).length, options);
        clusters = computeStatistics(clusters, options);
        return clusters;
    }

    function computeStatistics<T>(clusters : Cluster<T>[], options: GetClustersOptions<T>) : Cluster<T>[] {
        // UNDONE: clone
        for (var i = 0; i < clusters.length; i++) {
            var cluster = clusters[i];
            var max_dist = 0;
            for (var j = 0; j < cluster.data.length; j++) {
                var item = cluster.data[j];
                max_dist = Math.max(max_dist, options.distanceFunction(cluster.mean, options.vectorFunction(item)));
            }
            cluster.maxDistance = max_dist;
            cluster.count = cluster.data.length;
        }
        return clusters;
    }

    function createClusters<T>(data: T[], vectorDimension: number, options: GetClustersOptions<T>): Cluster<T>[] {
        var s = new Clustering.DBSCAN();
        var groups = 
            s.run(data, 
                options.desiredDistance, 
                options.idealGroupSize, 
                (a,b)=>options.distanceFunction(options.vectorFunction(a), options.vectorFunction(b))
            );

        var clusters: Cluster<T>[] = [];
        for (var i = 0; i < groups.length; i++) {
            var cluster = { mean: new Array(vectorDimension), data: [] };
            for (var j = 0; j < groups[i].length; j++) {
                cluster.data.push(data[groups[i][j]]);
            }
            clusters.push(cluster);
        }
        for (var i = 0; i < s.noise.length; i++) {
            clusters.push({ mean: new Array(vectorDimension), data: [data[s.noise[i]]] });
        }
        updateMeans(clusters, options.vectorFunction);

        return clusters;
    }

    function updateMeans<T>(clusters : Cluster<T>[], vectorFunction : VectorFunction<T>) {
        clusters.forEach(function (cluster) {
            updateMean(cluster, vectorFunction);
        });
    }


    function updateMean<T>(cluster : Cluster<T>, vectorFunction : VectorFunction<T>) {
        var newMean = new Array(cluster.mean.length);;

        var vectorizedData : number[][] = cluster.data.map(vectorFunction);

        for (var i = 0; i < cluster.mean.length; i++) {
            // average the "i"th column of the vectorized data
            newMean[i] = vectorizedData.map(item=>item[i]).reduce((a, b) => a + b) / vectorizedData.length;
        };

        cluster.mean = newMean;
    }

    function defaultVector<T>(x : T) { return x; }

    function defaultDistance(vector1 : number[], vector2 : number[]) : number {
        var sum = 0;

        for (var i = 0; i < vector1.length; i++) {
            sum = sum + Math.pow(vector1[i] - vector2[i],2)
        };

        return Math.sqrt(sum);
    }

}

module Clustering {
    // from: https://github.com/LukaszKrawczyk/density-clustering

    /**
     * DBSCAN - Density based clustering
     *
     * @author Lukasz Krawczyk <contact@lukaszkrawczyk.eu>
     * @copyright MIT
     */

    /**
     * DBSCAN class construcotr
     * @constructor
     *
     * @param {Array} dataset
     * @param {number} epsilon
     * @param {number} minPts
     * @param {function} distanceFunction
     * @returns {DBSCAN}
     */
    export class DBSCAN {
        dataset : any[];
        epsilon: number;
        minPts: number;
        distance: any;
        clusters: any[];
        noise: any[];
        _visited: any[];
        _assigned: any[];
        _datasetLength: number;

        constructor(dataset?, epsilon?, minPts?, distanceFunction?) {
            /** @type {Array} */
            this.dataset = [];
            /** @type {number} */
            this.epsilon = 1;
            /** @type {number} */
            this.minPts = 2;
            /** @type {function} */
            this.distance = this._euclideanDistance;
            /** @type {Array} */
            this.clusters = [];
            /** @type {Array} */
            this.noise = [];

            // temporary variables used during computation

            /** @type {Array} */
            this._visited = [];
            /** @type {Array} */
            this._assigned = [];
            /** @type {number} */
            this._datasetLength = 0;

            this._init(dataset, epsilon, minPts, distanceFunction);
        };

        /******************************************************************************/
        // public functions

        /**
         * Start clustering
         *
         * @param {Array} dataset
         * @param {number} epsilon
         * @param {number} minPts
         * @param {function} distanceFunction
         * @returns {undefined}
         * @access public
         */
        run(dataset?, epsilon?, minPts?, distanceFunction?) {
            this._init(dataset, epsilon, minPts, distanceFunction);

            for (var pointId = 0; pointId < this._datasetLength; pointId++) {
                // if point is not visited, check if it forms a cluster
                if (this._visited[pointId] !== 1) {
                    this._visited[pointId] = 1;

                    // if closest neighborhood is too small to form a cluster, mark as noise
                    var neighbors = this._regionQuery(pointId);

                    if (neighbors.length < this.minPts) {
                        this.noise.push(pointId);
                    } else {
                        // create new cluster and add point
                        var clusterId = this.clusters.length;
                        this.clusters.push([]);
                        this._addToCluster(pointId, clusterId);

                        this._expandCluster(clusterId, neighbors);
                    }
                }
            }

            return this.clusters;
        };

        /******************************************************************************/
        // protected functions

        /**
         * Set object properties
         *
         * @param {Array} dataset
         * @param {number} epsilon
         * @param {number} minPts
         * @param {function} distance
         * @returns {undefined}
         * @access protected
         */
        _init(dataset, epsilon, minPts, distance) {

            if (dataset) {

                if (!(dataset instanceof Array)) {
                    throw Error('Dataset must be of type array, ' +
                        typeof dataset + ' given');
                }

                this.dataset = dataset;
                this.clusters = [];
                this.noise = [];

                this._datasetLength = dataset.length;
                this._visited = new Array(this._datasetLength);
                this._assigned = new Array(this._datasetLength);
            }

            if (epsilon) {
                this.epsilon = epsilon;
            }

            if (minPts) {
                this.minPts = minPts;
            }

            if (distance) {
                this.distance = distance;
            }
        };

        /**
         * Expand cluster to closest points of given neighborhood
         *
         * @param {number} clusterId
         * @param {Array} neighbors
         * @returns {undefined}
         * @access protected
         */
        _expandCluster(clusterId, neighbors) {

            /**
             * It's very important to calculate length of neighbors array each time,
             * as the number of elements changes over time
             */
            for (var i = 0; i < neighbors.length; i++) {
                var pointId2 = neighbors[i];

                if (this._visited[pointId2] !== 1) {
                    this._visited[pointId2] = 1;
                    var neighbors2 = this._regionQuery(pointId2);

                    if (neighbors2.length >= this.minPts) {
                        neighbors = this._mergeArrays(neighbors, neighbors2);
                    }
                }

                // add to cluster
                if (this._assigned[pointId2] !== 1) {
                    this._addToCluster(pointId2, clusterId);
                }
            }
        };

        /**
         * Add new point to cluster
         *
         * @param {number} pointId
         * @param {number} clusterId
         */
        _addToCluster(pointId, clusterId) {
            this.clusters[clusterId].push(pointId);
            this._assigned[pointId] = 1;
        };

        /**
         * Find all neighbors around given point
         *
         * @param {number} pointId,
         * @param {number} epsilon
         * @returns {Array}
         * @access protected
         */
        _regionQuery(pointId) {
            var neighbors = [];

            for (var id = 0; id < this._datasetLength; id++) {
                var dist = this.distance(this.dataset[pointId], this.dataset[id]);
                if (dist < this.epsilon) {
                    neighbors.push(id);
                }
            }

            return neighbors;
        };

        /******************************************************************************/
        // helpers

        /**
         * @param {Array} a
         * @param {Array} b
         * @returns {Array}
         * @access protected
         */
        _mergeArrays(a, b) {
            var len = b.length;

            for (var i = 0; i < len; i++) {
                var P = b[i];
                if (a.indexOf(P) < 0) {
                    a.push(P);
                }
            }

            return a;
        };

        /**
         * Calculate euclidean distance in multidimensional space
         *
         * @param {Array} p
         * @param {Array} q
         * @returns {number}
         * @access protected
         */
        _euclideanDistance(p, q) {
            var sum = 0;
            var i = Math.min(p.length, q.length);

            while (i--) {
                sum += (p[i] - q[i]) * (p[i] - q[i]);
            }

            return Math.sqrt(sum);
        };
    }
}

module DimasKmeans {
// https://www.npmjs.com/package/dimas-kmeans + fixes/typescript :)
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
        options? : GetClustersOptions<T>) : {mean:T, maxDistance?:number, count?:number, data:T[]}[] {

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

        return computeStatistics(
            getClustersWithParams(data, numberOfDimensions, numberOfClusters, distanceFunction, vectorFunction, minMaxValues, maxIterations).clusters, 
            distanceFunction, 
            vectorFunction);
    }

    function computeStatistics(clusters, distanceFunction, vectorFunction)  {
        // UNDONE: clone
        for (var i = 0; i < clusters.length; i++) {
            var cluster = clusters[i];
            var max_dist = 0;
            for (var j = 0; j < cluster.data.length; j++) {
                var item = cluster.data[j];
                max_dist = Math.max(max_dist, distanceFunction(cluster.mean, vectorFunction(item)));
            }
            cluster.maxDistance = max_dist;
            cluster.count = cluster.data.length;
        }
        return clusters;
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