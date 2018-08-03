angular.module('eidApp.ChartModule.Controllers', []).controller('ChartController', function ($scope, $filter, $q, $http) {

    $scope.securities = [
        {
            id: 'GMKN',
            desc: 'Norilsk Nickel'
        },
        {
            id: 'LKOH',
            desc: 'Lukoil'
        },
        {
            id: 'NLMK',
            desc: 'NLMK'
        },
        {
            id: 'SBER',
            desc: 'Sberbank'
        },
        {
            id: 'MOEX',
            desc: 'MOEX'
        },
        {
            id: 'GAZP',
            desc: 'Gazprom'
        },
        {
            id: 'CHMF',
            desc: 'Severstal'
        },
        {
            id: 'FEES',
            desc: 'FSK EES'
        }];

    $scope.frames = [{
        id: '60',
        desc: 'Hour'
    }, {
        id: 'D',
        desc: 'Day'
        }];

    $scope.valueFilters = [
        {
            id: 1000000,
            desc: '1 000 000'
        },
        {
            id: 2000000,
            desc: '2 000 000'
        },
        {
            id: 3000000,
            desc: '3 000 000'
        },
        {
            id: 5000000,
            desc: '5 000 000'
        }];

    //$scope.security = "GMKN";

    $scope.frame = '60';

    $scope.from = new Date(2018, 6, 23);
    $scope.till = new Date(2018, 7, 3);

    $scope.updateSource = function (source, data) {

        $.each(data, function (index, obj) {
            source.push({
                x: new Date(obj.dateTime),
                y: obj.volume
            });
        });

    }

    $scope.$watch('security', function (newValue, oldValue) {

        var from_dt = $filter('date')($scope.from, "MM-dd-yyyy");
        var till_dt = $filter('date')($scope.till, "MM-dd-yyyy");

        var sales_url = '/api/DealGlobal/?security=' + newValue + '&from=' + from_dt + '&till=' + till_dt + '&operation=продажа' + '&interval=' + $scope.frame;

        var purchases_url = '/api/DealGlobal/?security=' + newValue + '&from=' + from_dt + '&till=' + till_dt + '&operation=купля' + '&interval=' + $scope.frame;

        if ($scope.valueFilter != undefined) {

            sales_url = sales_url + '&ValueFilter=' + $scope.valueFilter;
            purchases_url = purchases_url + '&ValueFilter=' + $scope.valueFilter;
        }

        var sale_request = $http.get(sales_url);
        var purchases_request = $http.get(purchases_url);

        $q.all([sale_request, purchases_request]).then(function (values) {
            obj1 = values[0].data;
            obj2 = values[1].data;

            sales_dps.splice(0, sales_dps.length);
            purchases_dps.splice(0, purchases_dps.length);

            $scope.updateSource(sales_dps, obj1);
            $scope.updateSource(purchases_dps, obj2);

            var arr = $.grep($scope.securities, function (s) {
                return s.id == $scope.security;
            });

            //chart.title.text = arr[0].desc;
            chart.title.options.text = arr[0].desc;

            chart.render();
        });

    });

});