# All Vagrant configuration is done below. The "2" in Vagrant.configure
# configures the configuration version (we support older styles for
# backwards compatibility). Please don't change it unless you know what
# you're doing.
Vagrant.configure("2") do |config|

    # Gets Ubuntu image
    config.vm.box = "ubuntu/bionic64"
    # Shares folder
    config.vm.synced_folder "../Backend", "/home/vagrant/Backend"
  
    config.vm.network "public_network", ip: "143.215.94.40"


    #Configures with Virtualbox
    config.vm.provider "virtualbox" do |vb|
  
    # Customize the amount of memory on the VM:
    vb.memory = "4096"
  end
  
  
    #Installs dependencies
    config.vm.provision "shell", inline: <<-SHELL
      apt-get update
      apt-get upgrade -y
      sudo apt install -y git
      mkdir Backend
      sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv 9DA31620334BD75D9DCB49F368818C72E52529D4
      echo "deb [ arch=amd64 ] https://repo.mongodb.org/apt/ubuntu bionic/mongodb-org/4.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-4.0.list
      sudo apt-get update
      sudo apt-get install -y mongodb-org
      wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
      sudo dpkg -i packages-microsoft-prod.deb
      sudo apt-get install -y apt-transport-https
      sudo apt-get update
      sudo apt-get install -y dotnet-sdk-2.1
      cd Backend
      dotnet restore
      mkdir -p /data/db
    SHELL
  end